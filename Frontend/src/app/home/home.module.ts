import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { IndexComponent } from './index/index.component';
import { HomeRoutingModule } from './home-routing.module';
import {HomeService} from './home.service';
import {NgxSpinnerModule} from 'ngx-spinner';
import {CategoryComponent} from './category/category.component';
import {SearchComponent} from './search/search.component';
import {FormsModule} from '@angular/forms';

@NgModule({
  declarations: [IndexComponent, CategoryComponent, SearchComponent],
  providers: [HomeService],
  imports: [
    CommonModule,
    RouterModule,
    HomeRoutingModule,
    NgxSpinnerModule,
    FormsModule
  ]
})
export class HomeModule { }
