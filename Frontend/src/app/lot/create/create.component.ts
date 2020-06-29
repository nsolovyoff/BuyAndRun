import { Component, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { finalize } from 'rxjs/operators'
import { AuthService } from '../../core/authentication/auth.service';
import { LotService } from '../lot.service';
import {Router} from '@angular/router';

@Component({
  selector: 'lot-index',
  templateUrl: './create.component.html',
  styleUrls: ['./create.component.scss']
})
export class CreateComponent implements OnInit {
  title : string;
  description: string;
  buyNowPrice: string;
  bid: string;
  expiring: string;
  categoryId: string;
  searchQuery: string;
  files = null;
  imageBase64: string;
  categories = null;

  constructor(private authService: AuthService, private lotService: LotService, private spinner: NgxSpinnerService, private router: Router) {
  }

  ngOnInit() {
    if (!this.authService.isAuthenticated())
      this.authService.login();

    this.lotService.getCategories()
      .pipe(finalize(() => {
      })).subscribe(
      result => {
        this.categories = result["data"];
      });
  }

  onCreate() {
    this.spinner.show();
    this.lotService.createLot(this.authService.authorizationHeaderValue, this.title, this.description, this.buyNowPrice, this.bid,
      this.expiring, this.categoryId, this.authService.name, this.imageBase64)
      .pipe(finalize(() => {
      })).subscribe(
      (result) => {
        this.spinner.hide();
        this.router.navigate(['/lot/' + result["id"]])
      });
  }

  onSearch() {
    this.router.navigate(['/search/' + this.searchQuery]);
  }

  getFiles(event) {
    this.files = event.target.files;
    var reader = new FileReader();
    reader.onload = this._handleReaderLoaded.bind(this);
    reader.readAsBinaryString(this.files[0]);
  }

  _handleReaderLoaded(readerEvt) {
    var binaryString = readerEvt.target.result;
    this.imageBase64 = "data:image/" + this.files[0].type + ";base64," + btoa(binaryString);  // Converting binary string data.
  }
}
