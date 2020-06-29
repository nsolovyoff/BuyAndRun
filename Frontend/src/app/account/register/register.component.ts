import { Component, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { finalize } from 'rxjs/operators'
import { AuthService } from '../../core/authentication/auth.service';
import { UserRegistration }    from '../../shared/models/user.registration';
import {Router} from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  success: boolean;
  error: string;
  userRegistration: UserRegistration = { name: '', email: '', password: '', userName: '', imageBase64: ''};
  submitted: boolean = false;
  files = null;

  constructor(private authService: AuthService, private spinner: NgxSpinnerService, private router: Router) {
  }

  ngOnInit() {
    if (this.authService.isAuthenticated())
      this.router.navigate(['/home']);
  }

  onSubmit() {
    this.spinner.show();

    this.authService.register(this.userRegistration)
      .pipe(finalize(() => {
      }))
      .subscribe(
      result => {
         if(result) {
           this.spinner.hide();
           this.success = true;
         }
      },
      error => {
        this.error = error;
      });
  }

  getFiles(event) {
    this.files = event.target.files;
    var reader = new FileReader();
    reader.onload = this._handleReaderLoaded.bind(this);
    reader.readAsBinaryString(this.files[0]);
  }

  _handleReaderLoaded(readerEvt) {
    var binaryString = readerEvt.target.result;
    this.userRegistration.imageBase64 = "data:image/" + this.files[0].type + ";base64," + btoa(binaryString);  // Converting binary string data.
  }
}
