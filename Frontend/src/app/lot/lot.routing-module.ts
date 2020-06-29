import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { Shell } from '../shell/shell.service';
import { IndexComponent } from './index/index.component';
import { AuthGuard } from '../core/authentication/auth.guard';
import {CreateComponent} from './create/create.component';


const routes: Routes = [
Shell.childRoutes([
    { path: 'lot/create', component: CreateComponent },
    { path: 'lot/:id', component: IndexComponent },
  ])
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: []
})
export class LotRoutingModule { }
