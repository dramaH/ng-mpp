import { NgModule, Injector, ComponentFactoryResolver } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { GanttComponent } from './gantt-chart.component';
import { FormsModule } from '@angular/forms';
import { NgZorroAntdModule } from 'ng-zorro-antd';
// import { NgZzjModule } from '../../components/ng-zzj.module';
import { ToolBarComponent } from './tool-bar/tool-bar.component';
import { TaskBoxComponent } from './task-box/task-box.component';
import { GanttBoxComponent } from './gantt-box/gantt-box.component';
import { ResizeBarComponent } from './resize-bar/resize-bar.component';
import { GanttHelperService } from './gantt-chart-service/gantt-helper.service';
import { GanttRequestService } from './gantt-chart-service/gantt-request.service';
import { GanttSize } from './gantt-chart-service/gantt.config';
import { PREVTYPE } from './src/api-public';
import { Project } from './src/lib/gantt.main';
import { GanttPipeModule } from './pipes/gantt-pipe.module';
import { PipeModule } from './src/lib/gantt.pipe';
import { DatepickerComponent } from './task-box/datepicker/datepicker.component';
import { ZzjModalModule } from './zzj-modal/zzj-modal.module';
// import { GanttComponent } from './gantt-chart.component';
// import { BuildLogService } from '../../services/project/build-logs/build-log.service';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    NgZorroAntdModule,
    GanttPipeModule,
    PipeModule,
    ZzjModalModule
    // NgZzjModule,
    // RouterModule.forChild([{ path: "", component: GanttComponent }])
  ],
  exports: [GanttComponent],
  declarations: [ResizeBarComponent, GanttComponent, ToolBarComponent, TaskBoxComponent, GanttBoxComponent, DatepickerComponent],
  providers: [GanttHelperService, GanttRequestService, {
    provide: 'PREVTYPE',
    useValue: PREVTYPE
  }, {
      provide: 'GanttSize',
      useValue: GanttSize
    }],
  entryComponents: [
    GanttComponent
  ]
})
export class GanttChartModule {
  constructor(private resolver: ComponentFactoryResolver) {
    Project.resolver = resolver;
  }
}
