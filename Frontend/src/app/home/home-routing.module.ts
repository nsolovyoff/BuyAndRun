import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { IndexComponent } from './index/index.component';
import { Shell } from './../shell/shell.service';
import {CategoryComponent} from './category/category.component';
import {SearchComponent} from './search/search.component';

const routes: Routes = [
  Shell.childRoutes([
    { path: '', redirectTo: '/home', pathMatch: 'full' },
    { path: 'home', component: IndexComponent },
    { path: 'category/:id', component: CategoryComponent },
    { path: 'search/:query', component: SearchComponent }
  ])
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: []
})
export class HomeRoutingModule { }
