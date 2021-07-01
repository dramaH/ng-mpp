import {
  Component,
  OnInit,
  ViewContainerRef,
  ViewChild,
  ComponentRef,
} from '@angular/core';
import { NzMessageService } from 'ng-zorro-antd';
import {
  GanttComponent,
  GanttProjectModel,
  XmppOptions,
  Xmpp,
  EXTENDATTRS,
  IMPPProject,
  Project,
  XmppTask,
  IXmpp,
  XmppExceptDate,
} from 'projects/zzj/zzj-xmpp/src/public-api';
import moment from 'moment';
import { GanttRequestService } from '../gantt-request.service';
import { environment } from 'src/environments/environment';
import * as uuid from 'uuid';

@Component({
  selector: 'app-xmpp-demo',
  templateUrl: './xmpp-demo.component.html',
  styleUrls: ['./xmpp-demo.component.less'],
})
export class XmppDemoComponent implements OnInit {
  constructor(
    private ganttRequestSev: GanttRequestService,
    private message: NzMessageService
  ) {}
  @ViewChild('ganttContainer', { read: ViewContainerRef, static: false })
  container: ViewContainerRef;
  public uploadForm: {
    uploadTitle: string;
    uploadMMP: File;
  };
  extendAttrs: any;
  tasks: any;
  ganttInfo: any;
  isOkLoading: boolean;
  uploadProjectView: boolean;
  ganttCreateView: boolean;
  public createForm: GanttProjectModel;
  option: XmppOptions;
  initStyle = {
    width: 1400,
    height: 800,
    top: 120,
    left: 120,
  };
  Gantt: GanttComponent;
  Xmpp: Xmpp = new Xmpp();

  currentProjectId: string;
  currentHostUrl: string;
  currentComponetRef: ComponentRef<GanttComponent>;
  weekDaysSettingView = false;
  exceptionsSettingView = false;

  exceptionList: XmppExceptDate[] = [];
  loadingTextShow = false;

  isOpen = false;

  baseZoom = 1;

  settingExceptions() {
    console.log(this.Xmpp.calendar.exceptDate);
    this.exceptionList = this.Xmpp.calendar.exceptDate;
    this.exceptionsSettingView = true;
  }
  addException() {
    this.exceptionList.push({
      name: '',
      fromDate: null,
      toDate: null,
    });
  }

  settingWeekDays() {
    console.log(this.Xmpp.calendar.weekDays);
    this.weekDaysSettingView = true;
  }

  download() {
    const objectUrl = URL;
    const aTag = document.createElement('a');
    document.body.appendChild(aTag);
    aTag.setAttribute('style', 'display:none');
    aTag.setAttribute('download', 'demo.zip');
    aTag.setAttribute('href', '../assets/demo/xmpp.zip');
    aTag.click();
    document.body.removeChild(aTag);
    // try {
    //     var elemIF = document.createElement("iframe");
    //     elemIF.src = objectUrl.replace(/\#/g, "%23");
    //     elemIF.style.display = "none";
    //     document.body.appendChild(elemIF);
    // } catch (e) {
    // }
  }
  ngOnInit() {
    this.createForm = new GanttProjectModel({
      title: '',
      dateFormat: null,
      author: name,
      company: '',
    });
    this.uploadForm = {
      uploadMMP: null,
      uploadTitle: '',
    };
    // 汇邻广场任务 23f1151f-155a-4a3b-a7cc-42c6aba31d14
    // 简单mpp
    // this.currentProjectId = 'fa1cdfa6-c564-4d04-8bd3-3c7297928f10';
    // this.currentProjectId = '2ebe5327-5774-4d0f-9846-4d4ea8ff253b';
    this.currentHostUrl = 'http://xmcec.test.spdio.com';
    this.currentProjectId = '39fd67c3-aaab-783f-f259-0813b46bb3b3';
    this.initProject(this.currentProjectId);
  }

  uploadCancel() {
    if (this.isOkLoading) {
      this.message.warning('请勿在上传完成前关闭弹窗');
    } else {
      this.uploadProjectView = false;
    }
  }

  async resizeEvent(style) {
    this.Gantt.updateSize();
    // await this.Gantt.initProject(this.option);
    // this.Xmpp.render()
  }

  /**
   * 获取进度项目所有的资源绑定
   */
  async getGanttExtendAttrs(mppExtendAttrs, ganttId) {
    // const extendAttrs = await this.ganttRequestSev.getGanttAttrs(gantt.id);
    // gantt.extendedAttributes = extendAttrs;
    const finder = mppExtendAttrs.find((attr) => {
      return attr.fieldID === EXTENDATTRS.binding.FieldID;
    });
    if (!finder) {
      const res = await this.ganttRequestSev.postGanttBindingAttr(ganttId);
      if (res.success) {
        this.message.success('资源创建成功');
        return true;
      } else {
        this.message.error('资源创建失败');
        return false;
      }
    } else {
      this.message.success('资源已绑定');
      // gantt.bindingInfo = {
      //   num: EXTENDATTRS.binding.FieldID,
      //   Id: finder.id
      // };
      return true;
    }
  }

  public async initProject(ganttId: string) {
    this.loadingTextShow = true;
    // 获取进度表
    const ganttInfo: IMPPProject = await this.ganttRequestSev.getGanttInfo(
      ganttId
    );
    this.ganttInfo = ganttInfo;
    // 获取进度表的拓展属性
    const extendAttrs = await this.ganttRequestSev.getGanttAttrs(ganttId);
    this.extendAttrs = extendAttrs;
    // 获取进度表的task列表
    const taskRes = await this.ganttRequestSev.getTasksList(ganttId);
    this.loadingTextShow = false;
    this.tasks = taskRes;
    // 定义列
    const option: XmppOptions = {
      mppInfo: ganttInfo,
      mppExtendAttrs: extendAttrs,
      mppTasks: taskRes,
      newLineVisible: true,
      columns: [
        {
          key: 'tags',
          width: 50,
          name: '标签',
          type: 'tags',
          resize: true,
        },
        {
          key: 'taskName',
          width: 120,
          name: '任务名称',
          type: 'input',
          resize: true,
        },
        {
          key: 'finishRate',
          width: 100,
          name: '完成率',
          type: 'input',
        },
        {
          key: 'startDate',
          width: 100,
          name: '计划开始时间',
          type: 'date',
        },
        {
          key: 'duration',
          width: 60,
          name: '工期',
          type: 'input',
        },
        {
          key: 'endDate',
          width: 100,
          name: '计划结束日期',
          type: 'date',
        },
        {
          key: 'prevRelationStr',
          width: 130,
          name: '前置任务',
          type: 'input',
        },
        {
          key: 'actualStartDate',
          width: 100,
          name: '实际开始日期',
          type: 'date',
        },
        {
          key: 'actualEndDate',
          width: 100,
          name: '实际结束日期',
          type: 'date',
        },
        {
          key: 'assignmentsStr',
          width: 100,
          name: '资源名称',
          type: 'input',
        },
        {
          key: 'taskLevel',
          width: 100,
          name: '任务等级',
          type: 'text',
        },
        {
          key: 'isMilepost',
          width: 100,
          name: '里程碑',
          type: 'text',
        },
      ],
      color: {
        /** 计划时间颜色 */
        planColor: 'rgba(65,159,229, 1)',
        /** 任务为关键线路时，计划时间颜色 */
        planKeyColor: 'rgba(255, 128, 128, 1)',
        /** 任务为延期任务时，计划时间颜色 */
        // planDelayColor: 'rgba(255, 194, 22, 1)',
        /** 实际时间颜色 */
        Actualcolor: '#419fe8',
        /** 任务为关键线路时，实际时间颜色 */
        ActualkeyColor: 'red',
        /** 任务为延期任务时，实际时间颜色 */
        // ActualDelayColor: 'rgba(255, 194, 22, 1)',
        /** 箭头及连线颜色 */
        arrowColor: 'rgba(65,159,229, 1)',
        /** 任务为关键线路时，箭头及连线颜色 */
        arrowKeyColor: 'rgba(255, 128, 128, 1)',
        /** 例外日期颜色 */
        exceptDateColor: 'rgba(204, 204, 204, 0.3)',
      },
      size: {},
    };
    this.option = option;
    if (this.currentComponetRef) {
      this.currentComponetRef.destroy();
    }
    this.currentComponetRef = await Project.newProject(
      this.container,
      this.option
    );
    this.Gantt = this.currentComponetRef.instance;
    this.Xmpp = this.Gantt.Xmpp;
    this.Xmpp.draw.lineHeight = 15;
    this.Xmpp.draw.actualLineHeight = 6;
    this.Xmpp.task.selectedTaskColor = 'rgba(0, 202, 207, 0.1)';
    this.Xmpp.render();
    console.log(this.Xmpp.allTasks);

    this.Xmpp.addTaskEventListener('clickListener', (res) => {
      const taskdom = document.getElementsByClassName('task-list')[1];
      taskdom.scrollTo(
        this.Xmpp.draw.canvasInfo[res.task.id - 1].positionX - 50,
        taskdom.scrollTop
      );
    });

    this.Xmpp.addTaskEventListener('dblclickListener', () => {
      console.log('dblclick');
    });
  }

  setMilepost() {
    console.log(this.Xmpp.task.selectedTasks);
    this.Xmpp.task.selectedTasks.forEach((ts) => {
      ts.duration = 0;
      ts.isMilepost = true;
    });
    console.log(this.Xmpp.allTasks);
    this.Xmpp.render();
  }

  zoom(type) {
    switch (type) {
      case 'in':
        if (this.baseZoom > 0.5) {
          this.baseZoom = Number((this.baseZoom - 0.1).toFixed(1));
          this.Xmpp.draw.zoom(this.baseZoom);
        }

        break;
      case 'out':
        if (this.baseZoom < 1.3) {
          this.baseZoom = Number((this.baseZoom + 0.1).toFixed(1));
          this.Xmpp.draw.zoom(this.baseZoom);
          break;
        }

      default:
        break;
    }
    console.log(this.baseZoom);
  }

  /**
   * 设置工作周弹窗提交
   */
  async weekDaysSettingSubmit() {
    // 发送请求
    console.log(this.Xmpp.calendar.weekDays);

    await this.ganttRequestSev.updateWeekDays(
      this.Xmpp.calendar.calendarId,
      this.Xmpp.calendar.weekDays
    );
    // 重新渲染
    this.Xmpp.render();
    this.weekDaysSettingView = false;
  }

  async exceptionsSettingSubmit() {
    console.log(this.exceptionList);
    const updateEcps = [];
    const addEcps = [];
    this.exceptionList.forEach(async (ecp) => {
      if (ecp.id) {
        updateEcps.push(ecp);
      } else {
        ecp.parentId = this.Xmpp.calendar.calendarId;
        addEcps.push(ecp);
      }
    });
    // await this.ganttRequestSev.updateException(updateEcps);
    if (addEcps.length > 0) {
      await this.ganttRequestSev.addException(addEcps);
    }

    if (updateEcps.length > 0) {
      await this.ganttRequestSev.updateException(updateEcps);
    }

    this.Xmpp.render();
    this.exceptionsSettingView = false;
  }

  async deleteException(exception: XmppExceptDate, index: number) {
    if (exception.id) {
      await this.ganttRequestSev.deleteException(exception.id);
    }
    this.exceptionList.splice(index, 1);
    this.Xmpp.render();
  }

  /**
   * 选择mpp文件
   * @param event
   */
  public selectFile(event: any) {
    event = event || window.event;
    if (!event || !event.target || !event.target.files) {
      return;
    }
    const files = event.target.files;
    this.uploadForm.uploadMMP = files[0];
  }

  /**
   * 下载xml文件
   */
  public async downloadXML() {
    if (this.Xmpp.mpp.mppInfo) {
      const id = this.Xmpp.mpp.mppInfo.id;
      const url = environment.mppUrl + `/mpp/export/${id}`;
      const aTag = document.createElement('a');
      document.body.appendChild(aTag);
      aTag.setAttribute('style', 'display:none');
      aTag.setAttribute('download', name);
      aTag.setAttribute('href', url.replace(/\#/g, '%23'));
      aTag.click();
      document.body.removeChild(aTag);
    } else {
      this.message.error('未打开项目，请先打开一个项目');
    }
  }

  /**
   * 上传mpp
   */
  public async uploadMPPHandle() {
    if (this.uploadForm.uploadTitle === '') {
      this.message.error('请输入进度名称');
      return;
    }
    if (!this.uploadForm.uploadMMP) {
      this.message.error('请选择导入的project文件');
      return;
    }
    this.isOkLoading = true;
    console.log(this.uploadForm);
    let mppRes;

    const formDate = new FormData();
    // formDate.set("Id", "118130c1-388c-4046-9f8c-7031fc864111")
    formDate.set('file', this.uploadForm.uploadMMP);

    mppRes = await this.ganttRequestSev.uploadMMP(formDate);
    if (mppRes) {
      this.initProject(mppRes.id);
    }
    // if (mppRes) {
    //   console.log(mppRes);
    //   if (mppRes) {
    //     this.initProject(mppRes.id);
    //   }
    //   this.message.success('导入成功');
    //   this.uploadForm = {
    //     uploadMMP: null,
    //     uploadTitle: ''
    //   };
    // } else {
    //   this.message.error('导入失败');
    // }
    this.isOkLoading = false;
    this.uploadProjectView = false;
  }

  /**
   * 创建gantt
   */
  public createNewGantt() {
    this.ganttCreateView = true;
    const companyName = JSON.parse(
      window.localStorage.getItem('project')
    ).companyName;
    const name = JSON.parse(window.localStorage.getItem('APDInfo')).userName;
    this.createForm = new GanttProjectModel({
      dateFormat: null,
      author: name,
      company: companyName,
    });
  }

  /**
   * 创建弹窗提交
   */
  public async submitCreateGantt() {
    const createForm = this.createForm.toCreateJson();
    console.log(createForm);
    if (!createForm) {
      this.message.error('进度名称不能为空');
      return;
    }
    const ganttId = await this.ganttRequestSev.postGantt(createForm);
    if (ganttId) {
      this.message.success('创建成功');
      // const projectId = JSON.parse(window.localStorage.getItem('project')).id;
      // this.router.navigate([`/inner/project/${projectId}/model`], { queryParams: { ganttId } });
      this.ganttCreateView = false;
      // this.messageService.send({ opt: "addTask" })
    } else {
      this.message.error('创建失败');
    }
  }

  /**
   * 绑定测试
   */
  async bindTest() {
    const ganttId = this.Xmpp.mpp.mppInfo.id;
    const selectTasks = this.Xmpp.task.selectedTasks;
    const extendAttrValue =
      'dfjlsajflkdjsafj看你自己想绑什么，只要是json就行fdjfkldjafjlsjflkdsjf';
    let res;
    for (const task of selectTasks) {
      if (!task.customExtendAttr) {
        // 如果之前有绑定
        const param = {
          FieldID: EXTENDATTRS.binding.FieldID,
          Value: extendAttrValue,
        };
        res = await this.ganttRequestSev.bindTaskExtendedAttribute(
          ganttId,
          task.sqId,
          param
        );
        if (res) {
          this.message.success('绑定成功');
        } else {
          this.message.error('绑定失败');
          return;
        }
      } else {
        // 如果之前没有绑定
        const param = {
          FieldID: EXTENDATTRS.binding.FieldID,
          Value: extendAttrValue,
        };
        res = await this.ganttRequestSev.updateExtendedAttrbute(
          ganttId,
          task.sqId,
          task.customExtendAttr.id,
          param
        );
        if (res) {
          this.message.success('更新绑定成功');
        } else {
          this.message.error('更新绑定失败');
          return;
        }
      }
      task.customExtendAttr = res;
      // 绑完之后给它一个标签
      task.tags.push({
        key: 'bindings',
        iconType: 'sketch',
        name: '绑定构件',
      });

      // 更新一下这个任务
      this.ganttRequestSev.updateTask(
        this.Xmpp.mpp.mppInfo.id,
        task.sqId,
        task.toMpp()
      );
    }
  }

  /**
   * 升级任务
   */
  promoteTaskLevel() {
    const selectTasks = this.Xmpp.task.selectedTasks;
    this.Xmpp.task.promoteTaskLevel(selectTasks);
  }

  /**
   * 降级任务
   */
  depressTaskLevel() {
    const selectedTasks = this.Xmpp.task.selectedTasks;
    this.Xmpp.task.depressTaskLevel(selectedTasks);
  }

  deleteTask() {
    const selectedTasks = this.Xmpp.task.selectedTasks;
    for (const task of selectedTasks) {
      this.Xmpp.task.deleteTaskHandle([task.id]);
    }
  }

  /**
   * 保存
   * 保存处理东西很多，比较麻烦建议就用这个
   * @param xmpp
   * @param type
   */
  public async saveTasksHanle() {
    const allTasks = this.Xmpp.allTasks;
    console.log(allTasks);
    debugger;
    const taskHasSqlId = [];
    const alreadyAddTasks: XmppTask[] = [];
    const alreadyEditTasks: XmppTask[] = [];
    const alreadyDeleteTaskIds: string[] = this.Xmpp.task.deleteTasksSqlIdStore;
    this.Xmpp.globalLoading = true;

    this.Xmpp.mpp.ParentId2WBS();

    // 组装 alreadyAddTasks
    for (const task of allTasks) {
      if (task.sqId) {
        taskHasSqlId.push(task);
      } else {
        this.Xmpp.task.maxUID = this.Xmpp.task.maxUID + 1;
        task.uid = this.Xmpp.task.maxUID;
        alreadyAddTasks.push(task);
      }
    }

    // 组装 alreadyEditTasks
    for (const element of taskHasSqlId) {
      const defaultData = element.defaultData;
      console.log(defaultData);
      const dateKey = [
        'startDate',
        'endDate',
        'actualStartDate',
        'actualEndDate',
      ];
      const arrayKey = [
        'bindings',
        'childTaskID',
        'prevRelation',
        'tags',
        'assignments',
        'isMilepost',
        'taskLevel',
      ];
      for (const itemKey in defaultData) {
        // 工期变化不考虑，再次获取tasklist会重新计算工期
        if (itemKey === 'duration' || itemKey === 'actualDuration') {
          continue;
        }
        // 时间参数变化
        if (dateKey.indexOf(itemKey) !== -1) {
          const elementDate = moment(element[itemKey]).format('YYYY MM DD');
          const defaultDate = moment(defaultData[itemKey]).format('YYYY MM DD');
          if (elementDate !== defaultDate) {
            const datefinder = alreadyEditTasks.findIndex((task) => {
              return task.sqId === element.sqId;
            });
            if (datefinder === -1) {
              alreadyEditTasks.push(element);
            }
          }
          continue;
        }
        // 数组类型参数变化
        if (arrayKey.indexOf(itemKey) !== -1) {
          console.log(itemKey);
          const elementArray = element[itemKey]
          ? element[itemKey].toString()
          : null;
          const defaultArray = defaultData[itemKey].toString();
          if (elementArray !== defaultArray) {
            const arrayfinder = alreadyEditTasks.findIndex((task) => {
              return task.sqId === element.sqId;
            });
            if (arrayfinder === -1) {
              alreadyEditTasks.push(element);
            }
          }
          continue;
        }

        // 普通参数变化
        if (element[itemKey] !== defaultData[itemKey]) {
          const normalfinder = alreadyEditTasks.findIndex((task) => {
            return task.sqId === element.sqId;
          });
          if (normalfinder === -1) {
            alreadyEditTasks.push(element);
          }
        }
      }
    }

    // 查询id是否连续
    const finder = allTasks.find((task, index) => {
      return task.id !== index + 1;
    });

    if (finder) {
      // 有错乱
      console.warn(finder);
      this.message.error('orderId重复');
    } else {
      const tasks = alreadyAddTasks.concat(alreadyEditTasks);
      // 整理
      const paramJson = [];
      alreadyAddTasks.forEach((task) => {
        task.sqId = uuid.v4();
        paramJson.push({
          op: 'create',
          type: 'task',
          data: task.toMpp(),
        });
      });
      alreadyEditTasks.forEach((task) => {
        paramJson.push({
          op: 'update',
          type: 'task',
          data: task.toMpp(),
        });
      });
      alreadyDeleteTaskIds.forEach((sqid) => {
        paramJson.push({
          op: 'delete',
          type: 'task',
          data: { Id: sqid },
        });
      });

      this.dealwithLinkAndAssignment(this.Xmpp, tasks, paramJson);
      this.dealwithResource(this.Xmpp, tasks, paramJson);
      console.log(paramJson);

      if (paramJson.length === 0) {
        this.message.success('提交成功');
        this.Xmpp.globalLoading = false;
        return;
      }
      const res = await this.ganttRequestSev.putTasks(
        this.Xmpp.mpp.mppInfo.id,
        paramJson
      );
      if (res) {
        this.message.success('提交成功');
      } else {
        this.message.success('提交成功');
      }
      this.Xmpp.globalLoading = false;
    }
  }

  dealwithResource(xmpp: Xmpp, tasks: XmppTask[], paramJson: any[]) {
    this.Xmpp.mpp.mppInfo.resources.forEach((rs) => {
      if (!rs.id) {
        paramJson.push({
          op: 'create',
          type: 'resource',
          data: {
            UID: rs.uid,
            Name: rs.name,
            Start: rs.start,
            Finish: rs.finish,
            ParentId: rs.parentId,
          },
        });
      }
    });
  }

  dealwithLinkAndAssignment(xmpp: Xmpp, tasks: XmppTask[], paramJson: any[]) {
    let maxAssignmentUid = this.Xmpp.mpp.findMaxUid(
      this.Xmpp.mpp.mppInfo.assignments
    );
    for (const task of tasks) {
      const relation = task.prevRelation;
      if (
        JSON.stringify(relation) !==
        JSON.stringify(task.defaultData.prevRelation)
      ) {
        // 如果有新增的link，则直接删除该任务所有link
        task.defaultData.prevRelation.forEach((rl) => {
          paramJson.push({
            op: 'delete',
            type: 'link',
            data: {
              Id: rl.id,
              ParentId: task.sqId,
            },
          });
        });
        task.prevRelation.forEach((rl) => {
          paramJson.push({
            op: 'create',
            type: 'link',
            data: {
              PredecessorUID: xmpp.allTasks[rl.prevId - 1].uid,
              ParentId: task.sqId,
              Type: rl.relation,
              LinkLag: rl.delay ? rl.delay * 8 * 600 : 0,
            },
          });
        });
      }

      if (
        JSON.stringify(task.assignments) !==
        JSON.stringify(task.defaultData.assignments)
      ) {
        const deleteAssignments = this.Xmpp.mpp.mppInfo.assignments.filter(
          (as) => as.taskUID === task.uid
        );
        deleteAssignments.forEach((as) => {
          paramJson.push({
            op: 'delete',
            type: 'assignment',
            data: {
              Id: as.id,
              ParentId: as.parentId,
            },
          });
        });
        task.assignments.forEach((as) => {
          maxAssignmentUid = maxAssignmentUid + 1;
          paramJson.push({
            op: 'create',
            type: 'assignment',
            data: {
              UID: maxAssignmentUid,
              ParentId: as.parentId,
              Start: as.start,
              Finish: as.finish,
              TaskUID: as.taskUID,
              ResourceUID: as.resourceUID,
            },
          });
        });
      }
    }
  }

  blur(e) {
    console.log(111);
  }

  /**
   * 插入任务
   * @param Gantt
   * @param taskParam
   */
  addTaskHandle() {
    this.Xmpp.task.addTask();
    console.log(this.Xmpp.allTasks);
    this.Xmpp.task.loopAllTasksId();
    this.Gantt.updateSize();
  }
}
