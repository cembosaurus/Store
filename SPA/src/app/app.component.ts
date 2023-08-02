import { Component } from '@angular/core';
import { AuthService } from '../app/_services/auth.service';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'SPA';

  constructor( private authService: AuthService) { }



  loggedIn() {
    return this.authService.isLoggedIn();
  }


}
