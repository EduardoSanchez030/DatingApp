import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
 // @Input() valuesFromHome: any;
  @Output() cancelRegister = new EventEmitter();
  model: any = {};

  constructor(private authServive: AuthService,
    private alertify: AlertifyService) { }

  ngOnInit() {
  }

  register() {
   this.authServive.register(this.model).subscribe(() => {
    this.alertify.sueccess('registration successful');
   }, error => {
    this.alertify.error(error);
   });
  }

  cancel() {
    this.cancelRegister.emit(false);
  }

}
