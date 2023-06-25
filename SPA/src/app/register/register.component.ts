import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { User } from '../_models/user';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  _user: User = { id: 0, name: "", password: ""};
  @Output() canceFromRegister = new EventEmitter();

  constructor(private service: AuthService) { }

  ngOnInit() {
  }

  register() {
    this.service.register(this._user)
      .subscribe(() => {
        console.log('User ' + this._user.name + ' was registered successfuly !');
      }, error => {
        console.error(error);
      });
  }

  cancel() {
    this.canceFromRegister.emit(false);
    console.log('Cancelled...');
  }

}