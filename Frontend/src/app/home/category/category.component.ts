import { Component, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { HomeService } from '../home.service';
import {finalize} from 'rxjs/operators';
import {ActivatedRoute, Router} from '@angular/router';


@Component({
  selector: 'app-index',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.scss']
})
export class CategoryComponent implements OnInit {
  lots = null;
  categories = null;
  categoryId: string;
  searchQuery : string;
  constructor(private homeService: HomeService, private spinner: NgxSpinnerService,
              private activatedRoute: ActivatedRoute, private router: Router) { }

  ngOnInit() {
    this.categoryId = this.activatedRoute.snapshot.params['id'];

    this.spinner.show();
    this.homeService.getLots(1, 100)
      .pipe(finalize(() => {
        this.spinner.hide();
      })).subscribe(
        result => {
          this.lots = result["data"].filter(lot => lot["categoryId"] == this.categoryId);
        });

    this.homeService.getCategories()
      .pipe(finalize(() => {
      })).subscribe(
      result => {
        this.categories = result["data"];
      });
  }

  onSearch() {
    this.router.navigate(['/search/' + this.searchQuery]);
  }
}
