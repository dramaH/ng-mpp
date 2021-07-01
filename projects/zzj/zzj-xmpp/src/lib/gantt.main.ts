
import { ComponentFactoryResolver, ComponentFactory, ComponentRef, Inject, Injector, ViewContainerRef, ElementRef } from '@angular/core';
import { GanttComponent } from './gantt-chart.component';
// @dynamic
export class Project {
    static resolver: ComponentFactoryResolver;
    static newProject(container: ViewContainerRef, options) {
        const factory: ComponentFactory<GanttComponent> = Project.resolver.resolveComponentFactory(GanttComponent);
        const componentRef: ComponentRef<GanttComponent> = container.createComponent(factory);
        componentRef.instance.initProject(options);
        return componentRef.instance;
    }
}
