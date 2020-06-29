import { Component, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { HomeService } from '../home.service';
import {finalize} from 'rxjs/operators';
import {ActivatedRoute} from '@angular/router';


@Component({
  selector: 'app-index',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss']
})
export class SearchComponent implements OnInit {
  lots = null;
  categories = null;
  searchQuery: string;
  formSearchQuery = null;
  constructor(private homeService: HomeService, private spinner: NgxSpinnerService,
              private activatedRoute: ActivatedRoute) { }

  ngOnInit() {
    this.searchQuery = this.activatedRoute.snapshot.params['query'];

    this.spinner.show();
    this.onSearch();

    this.homeService.getCategories()
      .pipe(finalize(() => {
      })).subscribe(
      result => {
        this.categories = result["data"];
      });
  }

  onSearch() {
    if (this.formSearchQuery)
      this.searchQuery = this.formSearchQuery;

    this.homeService.searchLots(this.searchQuery)
      .pipe(finalize(() => {
        this.spinner.hide();
      })).subscribe(
      result => {
        this.lots = result;
      });
  }
}
