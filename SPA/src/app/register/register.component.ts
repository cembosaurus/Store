import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  userDTO: any = {};
  @Output() canceFromRegister = new EventEmitter();

  constructor(private service: AuthService) { }

  ngOnInit() {
  }

  register() {
    this.service.register(this.userDTO)
      .subscribe(() => {
        console.log('User ' + this.userDTO.Name + ' was registered successfuly !');
      }, error => {
        console.error(error);
      });
  }

  cancel() {
    this.canceFromRegister.emit(false);
    console.log('Cancelled...');
  }

}
