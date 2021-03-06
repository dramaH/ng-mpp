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
    // ?????????????????? 23f1151f-155a-4a3b-a7cc-42c6aba31d14
    // ??????mpp
    // this.currentProjectId = 'fa1cdfa6-c564-4d04-8bd3-3c7297928f10';
    // this.currentProjectId = '2ebe5327-5774-4d0f-9846-4d4ea8ff253b';
    this.currentHostUrl = 'http://xmcec.test.spdio.com';
    this.currentProjectId = '39fd67c3-aaab-783f-f259-0813b46bb3b3';
    this.initProject(this.currentProjectId);
  }

  uploadCancel() {
    if (this.isOkLoading) {
      this.message.warning('????????????????????????????????????');
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
   * ???????????????????????????????????????
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
        this.message.success('??????????????????');
        return true;
      } else {
        this.message.error('??????????????????');
        return false;
      }
    } else {
      this.message.success('???????????????');
      // gantt.bindingInfo = {
      //   num: EXTENDATTRS.binding.FieldID,
      //   Id: finder.id
      // };
      return true;
    }
  }

  public async initProject(ganttId: string) {
    this.loadingTextShow = true;
    // ???????????????
    const ganttInfo: IMPPProject = await this.ganttRequestSev.getGanttInfo(
      ganttId
    );
    this.ganttInfo = ganttInfo;
    // ??????????????????????????????
    const extendAttrs = await this.ganttRequestSev.getGanttAttrs(ganttId);
    this.extendAttrs = extendAttrs;
    // ??????????????????task??????
    const taskRes = await this.ganttRequestSev.getTasksList(ganttId);
    this.loadingTextShow = false;
    this.tasks = taskRes;
    // ?????????
    const option: XmppOptions = {
      mppInfo: ganttInfo,
      mppExtendAttrs: extendAttrs,
      mppTasks: taskRes,
      newLineVisible: true,
      columns: [
        {
          key: 'tags',
          width: 50,
          name: '??????',
          type: 'tags',
          resize: true,
        },
        {
          key: 'taskName',
          width: 120,
          name: '????????????',
          type: 'input',
          resize: true,
        },
        {
          key: 'finishRate',
          width: 100,
          name: '?????????',
          type: 'input',
        },
        {
          key: 'startDate',
          width: 100,
          name: '??????????????????',
          type: 'date',
        },
        {
          key: 'duration',
          width: 60,
          name: '??????',
          type: 'input',
        },
        {
          key: 'endDate',
          width: 100,
          name: '??????????????????',
          type: 'date',
        },
        {
          key: 'prevRelationStr',
          width: 130,
          name: '????????????',
          type: 'input',
        },
        {
          key: 'actualStartDate',
          width: 100,
          name: '??????????????????',
          type: 'date',
        },
        {
          key: 'actualEndDate',
          width: 100,
          name: '??????????????????',
          type: 'date',
        },
        {
          key: 'assignmentsStr',
          width: 100,
          name: '????????????',
          type: 'input',
        },
        {
          key: 'taskLevel',
          width: 100,
          name: '????????????',
          type: 'text',
        },
        {
          key: 'isMilepost',
          width: 100,
          name: '?????????',
          type: 'text',
        },
      ],
      color: {
        /** ?????????????????? */
        planColor: 'rgba(65,159,229, 1)',
        /** ????????????????????????????????????????????? */
        planKeyColor: 'rgba(255, 128, 128, 1)',
        /** ????????????????????????????????????????????? */
        // planDelayColor: 'rgba(255, 194, 22, 1)',
        /** ?????????????????? */
        Actualcolor: '#419fe8',
        /** ????????????????????????????????????????????? */
        ActualkeyColor: 'red',
        /** ????????????????????????????????????????????? */
        // ActualDelayColor: 'rgba(255, 194, 22, 1)',
        /** ????????????????????? */
        arrowColor: 'rgba(65,159,229, 1)',
        /** ???????????????????????????????????????????????? */
        arrowKeyColor: 'rgba(255, 128, 128, 1)',
        /** ?????????????????? */
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
   * ???????????????????????????
   */
  async weekDaysSettingSubmit() {
    // ????????????
    console.log(this.Xmpp.calendar.weekDays);

    await this.ganttRequestSev.updateWeekDays(
      this.Xmpp.calendar.calendarId,
      this.Xmpp.calendar.weekDays
    );
    // ????????????
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
   * ??????mpp??????
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
   * ??????xml??????
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
      this.message.error('??????????????????????????????????????????');
    }
  }

  /**
   * ??????mpp
   */
  public async uploadMPPHandle() {
    if (this.uploadForm.uploadTitle === '') {
      this.message.error('?????????????????????');
      return;
    }
    if (!this.uploadForm.uploadMMP) {
      this.message.error('??????????????????project??????');
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
    //   this.message.success('????????????');
    //   this.uploadForm = {
    //     uploadMMP: null,
    //     uploadTitle: ''
    //   };
    // } else {
    //   this.message.error('????????????');
    // }
    this.isOkLoading = false;
    this.uploadProjectView = false;
  }

  /**
   * ??????gantt
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
   * ??????????????????
   */
  public async submitCreateGantt() {
    const createForm = this.createForm.toCreateJson();
    console.log(createForm);
    if (!createForm) {
      this.message.error('????????????????????????');
      return;
    }
    const ganttId = await this.ganttRequestSev.postGantt(createForm);
    if (ganttId) {
      this.message.success('????????????');
      // const projectId = JSON.parse(window.localStorage.getItem('project')).id;
      // this.router.navigate([`/inner/project/${projectId}/model`], { queryParams: { ganttId } });
      this.ganttCreateView = false;
      // this.messageService.send({ opt: "addTask" })
    } else {
      this.message.error('????????????');
    }
  }

  /**
   * ????????????
   */
  async bindTest() {
    const ganttId = this.Xmpp.mpp.mppInfo.id;
    const selectTasks = this.Xmpp.task.selectedTasks;
    const extendAttrValue =
      'dfjlsajflkdjsafj????????????????????????????????????json??????fdjfkldjafjlsjflkdsjf';
    let res;
    for (const task of selectTasks) {
      if (!task.customExtendAttr) {
        // ?????????????????????
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
          this.message.success('????????????');
        } else {
          this.message.error('????????????');
          return;
        }
      } else {
        // ????????????????????????
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
          this.message.success('??????????????????');
        } else {
          this.message.error('??????????????????');
          return;
        }
      }
      task.customExtendAttr = res;
      // ??????????????????????????????
      task.tags.push({
        key: 'bindings',
        iconType: 'sketch',
        name: '????????????',
      });

      // ????????????????????????
      this.ganttRequestSev.updateTask(
        this.Xmpp.mpp.mppInfo.id,
        task.sqId,
        task.toMpp()
      );
    }
  }

  /**
   * ????????????
   */
  promoteTaskLevel() {
    const selectTasks = this.Xmpp.task.selectedTasks;
    this.Xmpp.task.promoteTaskLevel(selectTasks);
  }

  /**
   * ????????????
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
   * ??????
   * ?????????????????????????????????????????????????????????
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

    // ?????? alreadyAddTasks
    for (const task of allTasks) {
      if (task.sqId) {
        taskHasSqlId.push(task);
      } else {
        this.Xmpp.task.maxUID = this.Xmpp.task.maxUID + 1;
        task.uid = this.Xmpp.task.maxUID;
        alreadyAddTasks.push(task);
      }
    }

    // ?????? alreadyEditTasks
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
        // ????????????????????????????????????tasklist?????????????????????
        if (itemKey === 'duration' || itemKey === 'actualDuration') {
          continue;
        }
        // ??????????????????
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
        // ????????????????????????
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

        // ??????????????????
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

    // ??????id????????????
    const finder = allTasks.find((task, index) => {
      return task.id !== index + 1;
    });

    if (finder) {
      // ?????????
      console.warn(finder);
      this.message.error('orderId??????');
    } else {
      const tasks = alreadyAddTasks.concat(alreadyEditTasks);
      // ??????
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
        this.message.success('????????????');
        this.Xmpp.globalLoading = false;
        return;
      }
      const res = await this.ganttRequestSev.putTasks(
        this.Xmpp.mpp.mppInfo.id,
        paramJson
      );
      if (res) {
        this.message.success('????????????');
      } else {
        this.message.success('????????????');
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
        // ??????????????????link?????????????????????????????????link
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
   * ????????????
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
