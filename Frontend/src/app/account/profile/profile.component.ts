import { Component, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { finalize } from 'rxjs/operators'
import { AuthService } from '../../core/authentication/auth.service';
import { AccountService } from '../account.service';
import {ActivatedRoute, Router} from '@angular/router';

@Component({
  selector: 'lot-index',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  user = null;
  searchQuery: string;

  constructor(private authService: AuthService, private accountService: AccountService, private spinner: NgxSpinnerService,
              private activatedRoute: ActivatedRoute, private router: Router) {
  }

  ngOnInit() {
    if (!this.authService.isAuthenticated())
      this.authService.login();

    this.spinner.show();
    this.accountService.getUser(this.authService.authorizationHeaderValue, this.authService.name)
      .pipe(finalize(() => {
        this.spinner.hide();
      })).subscribe(
      result => {
        this.user = result;
      });
  }


  onDelete () {
    this.spinner.show();
    this.accountService.deleteUser(this.authService.authorizationHeaderValue, this.authService.name)
      .pipe(finalize(() => {
        this.spinner.hide()
      })).subscribe(
      () => {
        this.authService.signout();
        this.router.navigate(['/home']);
      });
  }


  onSearch() {
    this.router.navigate(['/search/' + this.searchQuery]);
  }
}
