import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule }   from '../shared/shared.module';
import { IndexComponent } from './index/index.component';
import { LotRoutingModule } from './lot.routing-module';
import {LotService} from './lot.service';
import {FormsModule} from '@angular/forms';
import {CreateComponent} from './create/create.component';

@NgModule({
  declarations: [IndexComponent, CreateComponent],
  providers: [LotService],
  imports: [
    CommonModule,
    LotRoutingModule,
    SharedModule,
    FormsModule
  ]
})
export class LotModule { }
