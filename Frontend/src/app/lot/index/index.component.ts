import { Component, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { finalize } from 'rxjs/operators'
import { AuthService } from '../../core/authentication/auth.service';
import { LotService } from '../lot.service';
import {ActivatedRoute, Router} from '@angular/router';

@Component({
  selector: 'lot-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.scss']
})
export class IndexComponent implements OnInit {
  lot = null;
  isWinner : boolean;
  isOwner : boolean;
  isExpired : boolean;
  isWrongBid : boolean;
  lotId : string;
  bid : string;
  ownerEmail : string;
  searchQuery : string;

  constructor(private authService: AuthService, private lotService: LotService, private spinner: NgxSpinnerService,
              private activatedRoute: ActivatedRoute, private router: Router) {
  }

  ngOnInit() {
    if (!this.authService.isAuthenticated())
      this.authService.login();

    this.lotId = this.activatedRoute.snapshot.params['id'];
    this.updateLot();
  }

  updateLot() {
    this.spinner.show();
    this.lotService.getLot(this.authService.authorizationHeaderValue, this.lotId)
      .pipe(finalize(() => {
        this.spinner.hide();
      })).subscribe(
      result => {
        this.lot = result;
        this.isOwner = this.authService.name == this.lot["user"];
        this.isExpired = new Date() > new Date(this.lot["expiring"]);
         this.isWinner = this.isExpired && this.lot["bidUser"] == this.authService.name && !this.isOwner;
        if (this.isWinner)
          this.onWin();
      });
  }

  onWin() {
    this.lotService.getOwner(this.authService.authorizationHeaderValue, this.lotId)
      .pipe(finalize(() => {
        this.spinner.hide();
      })).subscribe(
      result => {
        this.ownerEmail = result["email"];
      });
  }

  onMakeBid() {
    if (Number(this.bid) <= this.lot["bid"]) {
      this.isWrongBid = true;
      return;
    }

    this.spinner.show();
    this.lotService.makeBid(this.authService.authorizationHeaderValue, this.authService.name, this.bid, this.lotId)
      .pipe(finalize(() => {
      })).subscribe(
      (result) => {
        this.updateLot();
      });
  }

  onBuyNow() {
    this.spinner.show();
    this.lotService.buyNow(this.authService.authorizationHeaderValue, this.lotId)
      .pipe(finalize(() => {
      })).subscribe(
      () => {
        this.updateLot();
      });
  }

  onDelete() {
    this.spinner.show();
    this.lotService.deleteLot(this.authService.authorizationHeaderValue, this.lotId)
      .pipe(finalize(() => {
        this.spinner.hide()
      })).subscribe(
      () => {
        this.router.navigate(['/home']);
      });
  }

  onSearch() {
    this.router.navigate(['/search/' + this.searchQuery]);
  }
}
