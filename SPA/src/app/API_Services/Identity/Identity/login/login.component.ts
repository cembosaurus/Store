import { Component } from '@angular/core';
import { AuthService } from '../../../../_services/auth.service';
import { User } from '../../../../_models/user';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  user: User = { id: 0, name: "", password: ""};
  registerMode = false;

  constructor(private authService: AuthService) { }

  ngOnInit() {
    this.user = this.getCurentUser();
  }

  login() {
    this.authService.login(this.user)
      .subscribe((response: any) => {
      if(!response.status)
      {
        alert(" LogIn for user '" + this.user.name + "' failed !\n" + response.message);
      }
    });
  }

  loggedIn() {
    return this.authService.isLoggedIn();
  }

  logOut() {
    localStorage.removeItem('token');
  }
  

  registerToggle() {
    this.registerMode = true;
  }

  cancelRegister(eventFromRegister: boolean) {
    this.registerMode = eventFromRegister;
  }

  getCurentUser()
  {
    return this.authService.getCurentUser();
  }
}
