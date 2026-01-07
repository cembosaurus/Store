(() => {
  const statusEl = document.getElementById("status");
  const debugEl = document.getElementById("debug");

  const tabLogin = document.getElementById("tab-login");
  const tabRegister = document.getElementById("tab-register");
  const panelLogin = document.getElementById("panel-login");
  const panelRegister = document.getElementById("panel-register");

  const loginForm = document.getElementById("loginForm");
  const registerForm = document.getElementById("registerForm");

  const loginBtn = document.getElementById("loginBtn");
  const registerBtn = document.getElementById("registerBtn");
  const whoamiBtn = document.getElementById("whoamiBtn");

  const apiBaseUrl =
    document.querySelector('meta[name="api-base-url"]')?.content?.trim() ||
    window.location.origin;

  // IMPORTANT SECURITY NOTE:
  // - Prefer server-set HttpOnly cookies for auth tokens.
  // - If your API returns a JWT in JSON, keep it in memory (NOT localStorage).
  let inMemoryAccessToken = null;

  function setStatus(kind, msg) {
    statusEl.className = `status ${kind || ""}`.trim();
    statusEl.textContent = msg || "";
  }

  function setDebug(obj) {
    debugEl.textContent = obj ? JSON.stringify(obj, null, 2) : "";
  }

  function switchTab(which) {
    const login = which === "login";
    tabLogin.classList.toggle("active", login);
    tabRegister.classList.toggle("active", !login);

    tabLogin.setAttribute("aria-selected", String(login));
    tabRegister.setAttribute("aria-selected", String(!login));

    panelLogin.classList.toggle("hidden", !login);
    panelRegister.classList.toggle("hidden", login);

    setStatus("", "");
    setDebug(null);
  }

  tabLogin.addEventListener("click", () => switchTab("login"));
  tabRegister.addEventListener("click", () => switchTab("register"));

  function getFirstNonEmpty(fd, ...names) {
    for (const n of names) {
      const v = fd.get(n);
      if (v === null || v === undefined) continue;
      const s = String(v).trim();
      if (s) return s;
    }
    return "";
  }

  function validateName(name) {
    // FluentValidation: 2..30 (and not null/empty)
    if (!name) return "Name is required.";
    if (name.length < 2 || name.length > 30) return "Name length must be 2–30 characters.";
    return null;
  }

  function validatePassword(password) {
    // FluentValidation: 4..8 (and not null/empty)
    if (!password) return "Password is required.";
    if (password.length < 4 || password.length > 8) return "Password length must be 4–8 characters.";
    return null;
  }

  function safeClearInput(form, inputName) {
    const el = form?.querySelector?.(`input[name="${inputName}"]`);
    if (el) el.value = "";
  }

  async function readJsonSafe(resp) {
    const ct = resp.headers.get("content-type") || "";
    if (ct.includes("application/json")) return await resp.json();
    const text = await resp.text();
    return text ? { message: text } : null;
  }

  function extractToken(payload) {
    if (!payload || typeof payload !== "object") return null;
    return (
      payload.accessToken ||
      payload.token ||
      payload.jwt ||
      payload?.data?.accessToken ||
      payload?.data?.token ||
      null
    );
  }

  async function postJson(path, body, { signal } = {}) {
    const url = `${apiBaseUrl.replace(/\/$/, "")}${path}`;

    const headers = {
      "Content-Type": "application/json",
      "Accept": "application/json",
    };

    // If you're NOT using cookies and you DO use bearer tokens:
    if (inMemoryAccessToken) headers["Authorization"] = `Bearer ${inMemoryAccessToken}`;

    const resp = await fetch(url, {
      method: "POST",
      headers,
      // If backend uses HttpOnly cookies, you MUST include credentials
      credentials: "include",
      body: JSON.stringify(body),
      signal,
    });

    const payload = await readJsonSafe(resp);

    if (!resp.ok) {
      const msg =
        payload?.message ||
        payload?.title ||
        payload?.error ||
        (typeof payload === "string" ? payload : null) ||
        `Request failed (${resp.status})`;
      const err = new Error(msg);
      err.status = resp.status;
      err.payload = payload;
      throw err;
    }

    return payload;
  }

  async function getJson(path, { signal } = {}) {
    const url = `${apiBaseUrl.replace(/\/$/, "")}${path}`;

    const headers = { "Accept": "application/json" };
    if (inMemoryAccessToken) headers["Authorization"] = `Bearer ${inMemoryAccessToken}`;

    const resp = await fetch(url, {
      method: "GET",
      headers,
      credentials: "include",
      signal,
    });

    const payload = await readJsonSafe(resp);

    if (!resp.ok) {
      const msg = payload?.message || payload?.title || `Request failed (${resp.status})`;
      const err = new Error(msg);
      err.status = resp.status;
      err.payload = payload;
      throw err;
    }

    return payload;
  }

  // /Identity/register expects UserToRegisterDTO: { Name, Password }
  registerForm.addEventListener("submit", async (e) => {
    e.preventDefault();
    setStatus("", "");
    setDebug(null);

    const fd = new FormData(registerForm);

    // Support both your old field names + new "name"
    const name = getFirstNonEmpty(fd, "name", "userName", "identifier", "Name");
    const password = String(fd.get("password") || "");
    const confirmPassword = String(fd.get("confirmPassword") || "");

    const nameErr = validateName(name);
    if (nameErr) {
      setStatus("err", nameErr);
      return;
    }

    const passErr = validatePassword(password);
    if (passErr) {
      setStatus("err", passErr);
      return;
    }

    if (confirmPassword && password !== confirmPassword) {
      setStatus("err", "Passwords do not match.");
      return;
    }

    // Match DTO EXACTLY (don’t send extra fields)
    const payload = { Name: name, Password: password };

    const controller = new AbortController();
    registerBtn.disabled = true;

    try {
      setStatus("", "Registering…");
      const res = await postJson("/Identity/register", payload, { signal: controller.signal });
      setDebug(res);
      setStatus("ok", "Registered. You can now log in.");
      registerForm.reset();
    } catch (err) {
      setDebug(err.payload || { error: err.message, status: err.status });
      setStatus("err", err.message || "Registration failed.");
    } finally {
      // Clear secrets quickly
      safeClearInput(registerForm, "password");
      safeClearInput(registerForm, "confirmPassword");
      registerBtn.disabled = false;
    }
  });

  // /Identity/login expects UserToLoginDTO: { Name, Password }
  loginForm.addEventListener("submit", async (e) => {
    e.preventDefault();
    setStatus("", "");
    setDebug(null);

    const fd = new FormData(loginForm);

    // Support both old "identifier" and new "name"
    const name = getFirstNonEmpty(fd, "identifier", "name", "userName", "Name");
    const password = String(fd.get("password") || "");

    const nameErr = validateName(name);
    if (nameErr) {
      setStatus("err", nameErr);
      return;
    }

    const passErr = validatePassword(password);
    if (passErr) {
      setStatus("err", passErr);
      return;
    }

    // Match DTO EXACTLY
    const payload = { Name: name, Password: password };

    const controller = new AbortController();
    loginBtn.disabled = true;

    try {
      setStatus("", "Logging in…");
      const res = await postJson("/Identity/login", payload, { signal: controller.signal });
      setDebug(res);

      const token = extractToken(res);
      if (token) {
        inMemoryAccessToken = token; // memory-only
        setStatus("ok", "Logged in (token kept in memory).");
      } else {
        setStatus("ok", "Logged in.");
      }
    } catch (err) {
      setDebug(err.payload || { error: err.message, status: err.status });
      setStatus("err", err.message || "Login failed.");
    } finally {
      // Clear secrets quickly
      safeClearInput(loginForm, "password");
      loginBtn.disabled = false;
    }
  });

  // Optional: see if auth worked (cookie or token) via /User/current
  whoamiBtn.addEventListener("click", async () => {
    setStatus("", "");
    setDebug(null);

    try {
      setStatus("", "Loading current user…");
      const res = await getJson("/User/current");
      setDebug(res);
      setStatus("ok", "Fetched current user.");
    } catch (err) {
      setDebug(err.payload || { error: err.message, status: err.status });
      setStatus("err", err.message || "Not authenticated (or endpoint blocked).");
    }
  });

  // default tab
  switchTab("login");
})();
