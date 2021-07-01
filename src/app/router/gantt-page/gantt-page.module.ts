import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GanttPageComponent } from './gantt-page.component';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NgZorroAntdModule } from 'ng-zorro-antd';
// import { GanttChartModule } from 'src/app/components/gantt-chart/gantt-chart.module';
import { MarkdownModule } from 'ngx-markdown';
import { ExplainComponent } from './explain/explain.component';
import { ResizeComponent } from './zzj-resize-box/resize-box.component';
import { XmppDemoComponent } from './xmpp-demo/xmpp-demo.component';
import { GanttChartModule } from 'projects/zzj/zzj-xmpp/src/public-api';

@NgModule({
  declarations: [GanttPageComponent, ExplainComponent, ResizeComponent, XmppDemoComponent],
  imports: [
    CommonModule,
    FormsModule,
    GanttChartModule,
    NgZorroAntdModule,
    RouterModule.forChild([
      {
        path: 'demo',
        component: GanttPageComponent,
        children: [
          { path: '', component: XmppDemoComponent }
        ]
      },
      {
        path: 'readme',
        component: GanttPageComponent,
        children: [
          { path: ':type', component: ExplainComponent }
        ]
      }
    ]),
    MarkdownModule.forRoot()
  ]
})
export class GanttPageModule { }
