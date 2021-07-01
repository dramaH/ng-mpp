import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  { path: '', redirectTo: 'gantt/demo', pathMatch: 'full' },
  {
    path: 'gantt',
    loadChildren: './router/gantt-page/gantt-page.module#GanttPageModule'
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
