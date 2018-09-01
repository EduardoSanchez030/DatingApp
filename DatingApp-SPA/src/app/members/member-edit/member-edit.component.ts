import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { User } from '../../_models/user';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from '../../_services/alertify.service';
import { NgForm } from '@angular/forms';
import { THIS_EXPR } from '../../../../node_modules/@angular/compiler/src/output/output_ast';
import { UserService } from '../../_services/user.service';
import { AuthService } from '../../_services/auth.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  user: User;
  photoUrl: string;
  @ViewChild('editForm') editForm: NgForm;
  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any) {
    if (this.editForm.dirty) {
       $event.returnValue = true;
    }
  }

  constructor(private router: ActivatedRoute, private alertify: AlertifyService,
  private userService: UserService, private authService: AuthService) { }

  ngOnInit() {
    this.router.data.subscribe(data => {
      this.user = data['user'];
    });
    this.authService.currentPhotoUrl.subscribe(photoUrl => this.photoUrl = photoUrl);
  }

  updateUser() {
  this.userService.updateUser(this.authService.decodedToken.nameid, this.user).subscribe(next => {
    this.alertify.sueccess('Profile updated successfully');
    this.editForm.reset(this.user);
  }, error => {
    this.alertify.error(error);
  });
  }

  updateMainPhoto(photoUrl) {
    this.user.photoUrl = photoUrl;
  }

}
