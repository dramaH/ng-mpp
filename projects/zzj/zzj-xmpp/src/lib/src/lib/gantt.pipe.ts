import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RelationPipe } from './pipe/relation.pipe';



@NgModule({
    declarations: [RelationPipe],
    imports: [
        CommonModule
    ],
    exports: [RelationPipe]
})
export class PipeModule { }
