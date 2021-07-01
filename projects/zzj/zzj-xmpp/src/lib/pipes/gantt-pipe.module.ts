import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DatePipe } from './Date.pipe';

@NgModule({
  declarations: [DatePipe],
  imports: [
    CommonModule
  ],
  exports: [DatePipe]
})
export class GanttPipeModule { }
