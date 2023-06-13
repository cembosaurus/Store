import { AlertifyService } from './../_services/alertify.service';
import { AuthService } from './../_services/auth.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { User } from '../_models/user';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any = {};
  photoUrl: string = "";
  user: User | undefined;

  constructor(public authService: AuthService, private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {  }

  login() {
    this.authService.login(this.model).subscribe(
      next => {
      this.alertify.success('Logged in successfuly.');
    }, error => {
      this.alertify.error(error);
    }, () => {
      this.router.navigate(['/members']);
    });
  }

  loggedIn() {
    return this.authService.isLoggedIn();
    // const token = localStorage.getItem('token');
    // return !!token;                                       // ... coerces object to boolean - null = false otherwise true ...
  }

  logOut() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.authService.decodedToken = null;
    this.authService.currentUser = this.user;
    this.alertify.warning('Logged Out');
    this.router.navigate(['/home']);
  }


}