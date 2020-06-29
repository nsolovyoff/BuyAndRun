import { Component, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { HomeService } from '../home.service';
import {finalize} from 'rxjs/operators';
import {ActivatedRoute, Router} from '@angular/router';
import {filter} from 'rxjs/operators';
import {from} from 'rxjs';


@Component({
  selector: 'app-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.scss']
})
export class IndexComponent implements OnInit {
  lots = null;
  categories = null;
  page: number;
  pageCount: number;
  searchQuery: string;

  constructor(private homeService: HomeService, private spinner: NgxSpinnerService,
              private activatedRoute: ActivatedRoute, private router: Router) { }

  ngOnInit() {
    from(this.activatedRoute.queryParams).pipe(filter(params => params.page))
      .subscribe(result => {
        this.page = result.page;
        this.updateLots();
      });

    if(!this.page) {
      this.page = 1;
      this.updateLots();
    }

    this.homeService.getCategories()
      .pipe(finalize(() => {
      })).subscribe(
      result => {
        this.categories = result["data"];
      });
  }

  updateLots() {
    this.spinner.show();
    this.homeService.getLots(this.page, 8)
      .pipe(finalize(() => {
        this.spinner.hide();
      })).subscribe(
      result => {
        this.lots = result["data"];
        this.pageCount = result["pageCount"];
      });
  }

  getPage(page: number): number {
    return page + Number(1);
  }

  onSearch() {
    this.router.navigate(['/search/' + this.searchQuery]);
  }
}
