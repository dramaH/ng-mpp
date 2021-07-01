import { Component, OnInit, Input, ViewChild, ElementRef, Inject } from '@angular/core';
import { Xmpp } from '../src/api-public';

@Component({
  selector: 'app-gantt-box',
  templateUrl: './gantt-box.component.html',
  styleUrls: ['./gantt-box.component.scss']
})
export class GanttBoxComponent implements OnInit {
  @ViewChild('myCanvas', { static: false }) public canvasRef: ElementRef;
  @ViewChild('maskCanvas', { static: false }) public maskCanvas: ElementRef;
  // @Input() public showTask: any;
  @Input() public canvasInfo: any;
  // public panelWidth: any;
  // @Input() allTasks :any;
  @Input() Xmpp: Xmpp;

  public weeksArry: any = [];
  public weeksWidth: any;
  public calendarWidth: number;
  public canvasWidth = 800;
  public loadingShow = false;
  public constructor() {

  }

  public ngOnInit() {
    this.Xmpp.task.canvasInfoListener.subscribe(res => {
      this.changeWidth();
    });
    // this.Xmpp.addGanttEventListener('mousedownListener', res => {
    //   // console.log('down');
    //   // console.log(res);
    // });
    // this.Xmpp.addGanttEventListener('mouseupListener', res => {
    //   // console.log('up');
    //   // console.log(res);
    // });
    // this.Xmpp.addGanttEventListener('contextmenuListener', res => {
    //   // console.log('右键');
    //   // console.log(res);
    // });

  }

  doMouseMove(event) {
    console.log(event);
  }

  public changeWidth() {
    // 日历宽度
    this.loadingShow = true;
    this.weeksArry = this.Xmpp.calendar.weeksArry;
    this.calendarWidth = this.Xmpp.calendar.calenderWidth;
    setTimeout(() => {
      this.loadingShow = false;
      this.drawCanvas();
    }, 200);
  }


  public boxScroll(e) {
    this.Xmpp.draw.canvasLeftHide = e.target.scrollLeft * 2;
    this.drawCanvas();
  }

  // 清空画布
  public cleanCanvas = () => {
    const ctx: CanvasRenderingContext2D =
      this.canvasRef.nativeElement.getContext('2d');

    ctx.clearRect(0, 0, this.Xmpp.draw.canvasWidth, this.Xmpp.draw.canvasHeight);
  }

  public drawCanvas() {
    console.log('start---drawCanvas');
    // 绘制日历
    const canvasInfo = this.Xmpp.draw.canvasInfo;
    const actualCanvasInfo = this.Xmpp.draw.actualCanvasInfo;
    console.log(canvasInfo);

    const x = this.Xmpp.draw.canvasWidth;
    const y = this.Xmpp.draw.canvasHeight;
    this.canvasRef.nativeElement.setAttribute('width', x * 2);
    this.canvasRef.nativeElement.setAttribute('height', y * 2);
    this.canvasRef.nativeElement.style.width = x + 'px';
    this.canvasRef.nativeElement.style.height = y + 'px';

    const ctx: CanvasRenderingContext2D =
      this.canvasRef.nativeElement.getContext('2d');
    ctx.clearRect(0, 0, this.Xmpp.draw.canvasWidth, this.Xmpp.draw.canvasHeight);

    this.maskCanvas.nativeElement.setAttribute('width', x * 2);
    this.maskCanvas.nativeElement.setAttribute('height', y * 2);
    this.maskCanvas.nativeElement.style.width = x + 'px';
    this.maskCanvas.nativeElement.style.height = y + 'px';

    const ctxMask: CanvasRenderingContext2D =
      this.maskCanvas.nativeElement.getContext('2d');
    this.Xmpp.draw.ctxMask = ctxMask;

    if (canvasInfo && canvasInfo.length > 0) {
      // 绘制横道图
      this.Xmpp.draw.drawExceptArea(ctx);
      this.Xmpp.draw.drawTasks(ctx, false);
    }
    if (actualCanvasInfo && actualCanvasInfo.length > 0) {
      this.Xmpp.draw.drawTasks(ctx, true);
    }

  }

}
