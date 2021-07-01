import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ZzjModalComponent } from './zzj-modal.component';
import { OverlayModule } from '@angular/cdk/overlay';
import { FormsModule } from '@angular/forms';
import { PortalModule } from '@angular/cdk/portal';
import { NgZorroAntdModule } from 'ng-zorro-antd';

@NgModule({
  declarations: [ZzjModalComponent],
  imports: [
    CommonModule,
    OverlayModule,
    FormsModule,
    PortalModule,
    NgZorroAntdModule
  ],
  exports: [ZzjModalComponent]
})
export class ZzjModalModule { }
