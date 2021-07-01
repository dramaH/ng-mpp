import { Component, OnInit, ViewChild, ElementRef, Input, Inject, AfterViewInit, SimpleChanges, ComponentFactoryResolver, ViewContainerRef, ComponentFactory, ComponentRef } from '@angular/core';
import { NzMessageService } from 'ng-zorro-antd';
import moment from 'moment';
import { GanttHelperService } from './gantt-chart-service/gantt-helper.service';
import { GanttBoxComponent } from './gantt-box/gantt-box.component';
import { GanttRequestService } from './gantt-chart-service/gantt-request.service';
import { GanttSize } from './gantt-chart-service/gantt.config';
import { Subject, fromEvent } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { Xmpp, XmppOptions, IMPPProject, XmppTask, XmppWeekDayType } from './src/api-public';



@Component({
  selector: 'app-gantt-chart',
  templateUrl: './gantt-chart.component.html',
  styleUrls: ['./gantt-chart.component.scss']
})
export class GanttComponent implements AfterViewInit {
  @ViewChild('taskbox', { static: false }) public taskbox: ElementRef;
  @ViewChild('ganttContainer', { static: false }) public ganttContainer: ElementRef;
  @ViewChild('container', { static: false }) public container: ElementRef;

  @ViewChild(GanttBoxComponent, { static: false }) ganttPanel: GanttBoxComponent;

  Xmpp: Xmpp = new Xmpp();

  // @Input() mppExtendAttrs: IExtendAttr[] = [];
  // @Input() mppInfo: IMPPProject;
  // @Input() mppTasks: ITask[] = [];
  // @Input() mppOptions: IXmppOptions;

  public leftPanel: any = {};
  public rightPanel: any = {};
  // 初始化宽度
  // 进度id
  @Input() public ganttId: string;

  @Input() public mppShowBottomPanel = true;

  public isFold = false;

  // public rightPanel: any = {};
  public lastScrollTop: 0;
  public scrollListHeight = 0;
  public resizeBarWith = 3;
  public scrollBarWith = 6;
  public rowHeight = 36;
  // public bottomHeight = 230;

  // 新建project
  // public isOkLoading = false;
  // public ganttListView = false;
  // public ganttList = [];

  private searchTerms = new Subject<string>();
  public schedulePnlStyleModel: any = {
    width: 0
  };
  mppOptions: XmppOptions;

  public get topPnlStyle() {
    const container = this.ganttContainer ? this.ganttContainer.nativeElement.clientHeight : 875;
    return {
      height: container + 'px'
    };
  }

  // public get bottomPnlStyle() {
  //   return {
  //     height: this.bottomHeight + 'px'
  //   };
  // }
  public get leftPnlStyle() {
    return {
      width: this.leftPanel.width + 'px'
    };
  }
  public get rightPnlStyle() {
    return {
      width: this.rightPanel.width + 'px'
    };
  }



  // allTasks: GanttModel[] = [];
  public constructor(
    private elementRef: ElementRef,
    public ganttHelperService: GanttHelperService,
    private message: NzMessageService,
    @Inject('PREVTYPE') public PREVTYPE: any,
    private ganttRequestSev: GanttRequestService,
    public ele: ElementRef,
  ) {

  }



  // public ngOnChanges(changes: SimpleChanges): void {

  //   if (changes.mppOptions.currentValue) {
  //     // Gantt.column.setColumn(mppOptions.columns);
  //     // Gantt.draw.color = mppOptions.color;
  //     // setTimeout(() => {
  //     //   // 进度模块宽度
  //     //   this.schedulePnlStyleModel.width = this.container.nativeElement.clientWidth - 2;
  //     //   // 任务列表面板宽度
  //     //   this.leftPanel.width = (this.schedulePnlStyleModel.width - this.resizeBarWith - this.scrollBarWith) / 2;
  //     //   this.rightPanel.width = this.leftPanel.width;
  //     //   // this.taskPanel.panelWidth = this.leftPanel.width;
  //     //   // 进度canvas面板宽度
  //     //   // this.ganttPanel.panelWidth = this.leftPanel.width;
  //     //   Gantt.draw.canvasWidth = this.leftPanel.width;
  //     //   // 设置假task列表高度，用以获取虚拟滚动条
  //     //   if (Gantt.allTasks) {
  //     //     this.scrollListHeight = Gantt.allTasks.length * this.rowHeight + this.scrollBarWith;
  //     //   }

  //     //   this.initTasks();
  //     // }, 1000);
  //   }
  // }

  initProject(mppOptions: XmppOptions): Promise<XmppTask[]> {
    this.mppOptions = mppOptions;
    // 处理列
    this.Xmpp.column.setColumn(mppOptions.columns);
    // 颜色
    this.Xmpp.draw.color = mppOptions.color;
    // 拓展属性
    this.Xmpp.mpp.mppExtendAttrs = mppOptions.mppExtendAttrs;
    // 项目信息
    this.Xmpp.mpp.mppInfo = mppOptions.mppInfo;
    let rightScrollBarWith = 0;
    this.Xmpp.globalLoading = true;
    // 获取进度表的task列表
    // mppOptions.mppInfo.tasks = mppOptions.mppTasks;
    // this.getExceptions(ganttInfo);
    this.Xmpp.mpp.setMppInfo(mppOptions.mppInfo);
    this.Xmpp.mpp.setMppTasks(mppOptions.mppTasks);

    return new Promise((resolve, reject) => {
      setTimeout(() => {
        const containerHeight = this.container.nativeElement.clientHeight;

        // 设置假task列表高度，用以获取虚拟滚动条
        if (mppOptions.mppTasks.length > 0) {
          const scrollListHeight = this.getScrollHeight();
          if (scrollListHeight < containerHeight - 50) {
            rightScrollBarWith = 0;
          } else {
            rightScrollBarWith = this.scrollBarWith;
          }
          this.scrollListHeight = scrollListHeight;
        }
        // 进度模块宽度
        this.schedulePnlStyleModel.width = this.container.nativeElement.clientWidth;
        // 任务列表面板宽度

        if (mppOptions.size.taskListInitWidth) {
          this.leftPanel.width = mppOptions.size.taskListInitWidth;
        } else {
          this.leftPanel.width = (this.schedulePnlStyleModel.width - this.resizeBarWith - rightScrollBarWith) / 2;
        }

        this.rightPanel.width = this.schedulePnlStyleModel.width - this.resizeBarWith - rightScrollBarWith - this.leftPanel.width;
        // this.taskPanel.panelWidth = this.leftPanel.width;
        // 进度canvas面板宽度
        // this.ganttPanel.panelWidth = this.leftPanel.width;
        this.Xmpp.draw.canvasWidth = this.rightPanel.width;

        // const height = this.taskbox.nativeElement.clientHeight;
        this.Xmpp.task.showTaskLength = Math.floor((containerHeight - 50) / GanttSize.taskHeight);
        // 50：日历高度； 20：底部滚动条高度
        this.Xmpp.draw.canvasHeight = containerHeight - 50 - this.scrollBarWith;
        // 渲染列表和canvas
        console.log(this.Xmpp.allTasks);
        // this.Xmpp.render();

        resolve(this.Xmpp.allTasks);
      }, 500);
    });
  }

  updateSize() {
    setTimeout(() => {
      let rightScrollBarWith = 0;
      const containerHeight = this.container.nativeElement.clientHeight;

      // 设置假task列表高度，用以获取虚拟滚动条
      if (this.Xmpp.allTasks.length > 0) {
        const scrollListHeight = this.getScrollHeight();
        if (scrollListHeight < containerHeight - 50) {
          rightScrollBarWith = 0;
        } else {
          rightScrollBarWith = this.scrollBarWith;
        }
        this.scrollListHeight = scrollListHeight + 20;
      }
      // 进度模块宽度
      this.schedulePnlStyleModel.width = this.container.nativeElement.clientWidth;
      // 任务列表面板宽度

      // if (this.Xmpp.size.taskListInitWidth) {
      //   this.leftPanel.width = this.Xmpp.size.taskListInitWidth;
      // } else {
      //   this.leftPanel.width = (this.schedulePnlStyleModel.width - this.resizeBarWith - rightScrollBarWith) / 2;
      // }

      this.rightPanel.width = this.schedulePnlStyleModel.width - this.resizeBarWith - rightScrollBarWith - this.leftPanel.width;
      // this.taskPanel.panelWidth = this.leftPanel.width;
      // 进度canvas面板宽度
      // this.ganttPanel.panelWidth = this.leftPanel.width;
      this.Xmpp.draw.canvasWidth = this.rightPanel.width;

      // const height = this.taskbox.nativeElement.clientHeight;
      this.Xmpp.task.showTaskLength = Math.floor((containerHeight - 50) / GanttSize.taskHeight);
      // 50：日历高度； 20：底部滚动条高度
      this.Xmpp.draw.canvasHeight = containerHeight - 50 - this.scrollBarWith;

      this.ganttPanel.drawCanvas();
    }, 500);

  }


  outerResizeCallback(e) {
    this.leftPanel.width = this.schedulePnlStyleModel.width - this.resizeBarWith - this.scrollBarWith - this.rightPanel.width;
  }

  updateTasks() {
    this.Xmpp.render();
  }

  public ngOnInit() {
    // const ganttId = this.activatedRoute.snapshot.queryParams.ganttId;
    this.searchTerms
      .pipe(
        // 请求防抖 300毫秒
        debounceTime(500),
        distinctUntilChanged())
      .subscribe(term => {
        this.Xmpp.draw.updateCanvasInfo();
      });
  }

  /**
   * 加载表格数据
   * @param ganttId
   */
  public async initTasks(mppOptions: XmppOptions) {


    // this.detailView = false;
  }


  // /**
  //  * 选择mpp文件
  //  * @param event
  //  */
  // public selectFile(event: any) {
  //   event = event || window.event;
  //   if (!event || !event.target || !event.target.files) {
  //     return;
  //   }
  //   const files = event.target.files;
  //   this.uploadForm.uploadMMP = files[0];
  // }

  // /**
  //  * 上传mpp
  //  */
  // public async uploadMPPHandle() {
  //   if (this.uploadForm.uploadTitle === '') {
  //     this.message.error('请输入进度名称');
  //     return;
  //   }
  //   if (!this.uploadForm.uploadMMP) {
  //     this.message.error('请选择导入的project文件');
  //     return;
  //   }
  //   this.isOkLoading = true;
  //   console.log(this.uploadForm);
  //   const mpp = await this.ganttRequestSev.uploadMMP(this.uploadForm);
  //   if (mpp) {
  //     console.log(mpp);
  //     // if (mpp) {
  //     //   this.initTasks(mpp.id);
  //     // }
  //     this.message.success('导入成功');
  //     this.uploadForm = {
  //       uploadMMP: null,
  //       uploadTitle: ''
  //     };
  //   } else {
  //     this.message.error('导入失败');
  //   }
  //   this.isOkLoading = false;
  //   this.uploadProjectView = false;
  // }

  /**
   * 下载xml文件
   */
  public async downloadXML() {
    if (this.Xmpp.mpp.mppInfo) {
      const id = this.Xmpp.mpp.mppInfo.id;
      const url = `http://localhost:5004/mpp/api/v1/mpp/export/${id}`;
      //FileHelper.download('Project', url);
    } else {
      this.message.error('未打开项目，请先打开一个项目');
    }
  }

  /**
   * 获取表格列表
   * @param gantt
   */
  async getGanttTaskList(gantt: IMPPProject) {
    const res = await this.ganttRequestSev.getTasksList(gantt.id);
    gantt.tasks = res;
  }



  resizeCallback($event) {
    // this.ganttPanel.panelWidth = this.rightPanel.width;
    this.Xmpp.draw.canvasWidth = this.rightPanel.width;
    this.ganttPanel.changeWidth();
  }

  // 进度模块
  public get schedulePnlStyle() {

    return {
      width: this.schedulePnlStyleModel.width + 'px',
      height: '100%'
    };
  }
  public dragstartHandler(e: any, model?: string) {
    e.stopPropagation();
    e.preventDefault();
    // console.log(e);
    let startX = e.clientX;
    const dragHandler = (ev) => {
      // console.log(e)
      ev.stopPropagation();
      ev.preventDefault();
      this.schedulePnlStyleModel.width -= (e.clientX - startX);
      startX = e.clientX;
    };
    console.log(this.schedulePnlStyleModel);
    const dragendHandler = () => {
      document.documentElement.removeEventListener('mousemove', dragHandler, false);
      document.documentElement.removeEventListener('mouseup', dragendHandler, false);
    };
    document.documentElement.addEventListener('mouseup', dragendHandler, false);
    document.documentElement.addEventListener('mousemove', dragHandler, false);

  }


  public ngAfterViewInit() {
    // fromEvent(this.container.nativeElement, 'resize')
    //   .pipe(
    //     debounceTime(300),
    //     distinctUntilChanged()
    //   )
    //   .subscribe((event) => {
    //     // 这里处理页面变化时的操作
    //     this.resize()
    //   });
  }

  async resize() {
    // await this.initProject(this.mppOptions);
    // this.Xmpp.render()
  }

  /**
   * 计算进度列表和canvas面板的高度
   * 触发方式：改变窗口大小、关闭底部panel
   */
  public refreshViewHeight() {
    // 50: toolbar高度
    const height = this.taskbox.nativeElement.clientHeight;
    this.Xmpp.task.showTaskLength = Math.ceil((height - 50) / GanttSize.taskHeight);
    // 50：日历高度； 20：底部滚动条高度
    this.Xmpp.draw.canvasHeight = this.taskbox.nativeElement.clientHeight - 50 - this.scrollBarWith;
    // 计算showTask
    this.Xmpp.task.updateTaskHandle();
    // 最后更新 canvas和日历(update canvas)
    this.Xmpp.draw.updateCanvasInfo();
  }

  // public switchBottom(type: string) {
  //   switch (type) {
  //     case 'down':
  //       this.bottomHeight = 40;
  //       this.isFold = true;
  //       setTimeout(() => {
  //         this.refreshViewHeight();
  //       }, 500);
  //       break;
  //     case 'up':
  //       this.bottomHeight = 300;
  //       this.isFold = false;
  //       setTimeout(() => {
  //         this.refreshViewHeight();
  //       }, 500);
  //       break;
  //     default:
  //       break;
  //   }

  // }

  public getScrollHeight() {
    let scrollListHeight = 0;
    if (this.Xmpp.allTasks) {
      // 36： 一个task的高度
      // 加50： 是最后一个输入行的高度
      scrollListHeight = this.Xmpp.task.getAllTaskAfterFold().length * (this.rowHeight + 1);
    }
    return scrollListHeight;
  }

  // 窗口缩放
  public windowResize(event) {
    this.refreshViewHeight();
    // tslint:disable-next-line:max-line-length
    this.rightPanel.width = this.container.nativeElement.clientWidth - this.leftPanel.width - this.resizeBarWith - this.scrollBarWith;
  }

  /**
   * 滚动条滚动事件
   * @param e any
   */
  public scrollHandler(e) {
    let IsDown = false;
    const scrollTop = e.target.scrollTop;
    scrollTop > this.lastScrollTop ? IsDown = true : IsDown = false;
    if (IsDown) {
      const targetTop = Math.ceil(scrollTop / this.rowHeight) * this.rowHeight;
      e.target.scrollTop = targetTop;
    } else {
      const targetTop = Math.floor(scrollTop / this.rowHeight) * this.rowHeight;
      e.target.scrollTop = targetTop;
    }
    this.lastScrollTop = scrollTop;
    const starIndex = Math.floor(this.lastScrollTop / this.rowHeight);
    // 更新tasklist和canvas
    this.Xmpp.task.startTaskIndex = starIndex;
    this.Xmpp.task.updateTaskHandle();

    this.Xmpp.draw.updateCanvasInfo();
    // 防抖
    // this.searchTerms.next(scrollTop.toString());
  }

  /**
   * 鼠标滚动事件
   * @param e
   * 控制假滚动条滚动
   */
  public mousewheelhandler(e) {
    const o = this.elementRef.nativeElement.querySelector('.scrollBar');
    o.scrollTop = o.scrollTop + e.deltaY / 100 * this.rowHeight;
  }

  newTaskBlur(e) {
    // console.log(this.elementRef.nativeElement.querySelector('.scrollBar').scrollHeight)
    const o = this.elementRef.nativeElement.querySelector('.scrollBar');
    o.scrollTop = this.lastScrollTop + 36;
    // this.mousewheelhandler(e)
  }

  // /**
  //  * 创建gantt
  //  */
  // public createNewGantt() {
  //   this.ganttCreateView = true;
  //   const companyName = JSON.parse(window.localStorage.getItem('project')).companyName;
  //   const name = JSON.parse(window.localStorage.getItem('APDInfo')).userName;
  //   this.createForm = new GanttProjectModel({
  //     dateFormat: null,
  //     author: name,
  //     company: companyName
  //   });
  // }

  /**
   * 新建项目
   */
  public changeStartDate(group: { name: string, startDate: any, endDate: any }) {
    const startDate = moment(group.startDate).unix();
    const endDate = moment(group.endDate).unix();
    if (!group.endDate) {
      group.endDate = startDate;
      return;
    } else {
      if (startDate > endDate) {
        group.endDate = startDate;
        return;
      }
    }
  }

  // public async submitCreateGantt() {
  //   const createForm = this.createForm.toCreateJson();
  //   console.log(createForm);
  //   if (!createForm) {
  //     this.message.error('进度名称不能为空');
  //     return;
  //   }
  //   const ganttId = await this.ganttRequestSev.postGantt(createForm);
  //   if (ganttId) {
  //     this.message.success('创建成功');
  //     // const projectId = JSON.parse(window.localStorage.getItem('project')).id;
  //     // this.router.navigate([`/inner/project/${projectId}/model`], { queryParams: { ganttId } });
  //     this.ganttCreateView = false;
  //     // this.messageService.send({ opt: "addTask" })
  //   } else {
  //     this.message.error('创建失败');
  //   }
  // }


  /**
   * 获取项目列表
   */
  // public async showGanttList() {
  //   const list = await this.ganttRequestSev.getGanttList();
  //   this.ganttList = [];
  //   list.forEach((element) => {
  //     this.ganttList.push({
  //       id: element.id,
  //       parentId: element.parentId,
  //       calendars: element.calendars,
  //       creationDate: element.creationDate,
  //       title: element.title,
  //       startDate: element.startDate,
  //       finishDate: element.finishDate
  //     });
  //   });
  //   console.log(this.ganttList);
  //   this.ganttListView = true;
  // }


  public deletDate(index: number, exceptDate) {
    exceptDate.splice(index, 1);
  }
  public addDate(exceptDate) {
    exceptDate.push(
      { name: '', startDate: null, endDate: null }
    );
  }
  public createMessage = (type, text) => {
    this.message.create(type, text);
  }

  public rulesForNewProject = (newGanttModel) => {
    if (newGanttModel.ganttName === '') {
      this.createMessage('error', '项目名称不能为空');
      return false;
    } else if (!newGanttModel.startDate) {
      this.createMessage('error', '开始时间不能为空');
      return false;
    } else if (!newGanttModel.endDate) {
      this.createMessage('error', '结束时间不能为空');
      return false;
    } else {
      if (newGanttModel.exceptDate.length > 0) {
        const checkName = newGanttModel.exceptDate.findIndex((element) => {
          return element.name === '';
        });
        const checkDate = newGanttModel.exceptDate.findIndex((element) => {
          return element.startDate == null;
        });
        if (checkName !== -1) {
          this.createMessage('error', '例外日期名称不能为空');
          return false;
        } else if (checkDate !== -1) {
          this.createMessage('error', '例外日期不能为空');
          return false;
        } else {
          return true;
        }
      } else {
        return true;
      }

    }
  }
}
