import { Component } from '@angular/core';
import { AuthService } from '../_services/auth.service';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  userDTO: any = {};
  registerMode = false;

  constructor(private authService: AuthService) { }

  ngOnInit() {
  }

  login() {
    this.authService.login(this.userDTO)
      .subscribe(
        next => {
        console.log('Login was successful');
        },
        error => {
          console.log(error);
        }
      );
  }

  loggedIn() {
    const token = localStorage.getItem('token');
    return !!token;
  }

  logOut() {
    localStorage.removeItem('token');
    console.log('Logged OUT !');
  }
  

  registerToggle() {
    this.registerMode = true;
  }

  cancelRegister(eventFromRegister: boolean) {
    this.registerMode = eventFromRegister;
  }
}
