import moment from 'moment';
import { Subject } from 'rxjs';
import { isNullOrUndefined } from 'util';
import { Xmpp } from './gantt.api';
import { prevType2Str } from './gantt.config';

export interface XmppOptions {
  mppInfo: IMPPProject;
  mppTasks: IMppTask[];
  mppExtendAttrs: IMppExtendAttr[];
  columns: XmppColumn[];
  newLineVisible?: boolean;
  color: {
    /** 计划时间颜色 */
    planColor: string;
    /** 任务为关键线路时，计划时间颜色 */
    planKeyColor: string;
    /** 任务为延期任务时，计划时间颜色 */
    planDelayColor?: string;
    /** 实际时间颜色 */
    Actualcolor: string;
    /** 任务为关键线路时，实际时间颜色 */
    ActualkeyColor: string;
    /** 任务为延期任务时，实际时间颜色 */
    ActualDelayColor?: string;
    /** 箭头及连线颜色 */
    arrowColor: string;
    /** 任务为关键线路时，箭头及连线颜色 */
    arrowKeyColor: string;
    /** 例外日期区域的颜色 */
    exceptDateColor: string;
  };
  size: {
    /** 左边列表框初始化宽度，默认为外层容器的一半 */
    taskListInitWidth?: number;
  };
}

export enum XmppWeekDayType {
  '日' = 1,
  '一',
  '二',
  '三',
  '四',
  '五',
  '六',
}

export class UInfoModel {
  public uuid: string;
  public isFinished = 0;
  public finishAt: any;
  public constructor(param: any) {
    param.isFinished && (this.isFinished = param.isFinished);
    param.finishAt && (this.finishAt = param.finishAt);
    param.uuid && (this.uuid = param.uuid);
  }
}

export interface XmppColumn {
  // 该列唯一key
  key:
    | string
    | 'tags'
    | 'taskName'
    | 'startDate'
    | 'duration'
    | 'endDate'
    | 'actualStartDate'
    | 'actualDuration'
    | 'actualEndDate'
    | 'prevRelationStr'
    | 'assignmentsStr'
    | 'finishRate'
    | 'taskLevel'
    | 'isMilepost';
  // 列名
  name: string;
  // 列宽
  width: number;
  // 该列ui组件类型
  type?: 'tags' | 'text' | 'input' | 'date' | 'checkbox';
  // 是否拖动
  resize?: boolean;
  // 是否可编辑
  isEdit?: boolean;
}

export interface IXmpp {
  allTasks: Array<XmppTask>;
  globalLoading: boolean;
  mpp: {
    mppInfo: IMPPProject;
    mppTasks: IMppTask[];
    mppExtendAttrs: IMppExtendAttr[];
    extraAttrMap: Map<string, any>;
  };
  task: {
    isFirstInit: boolean;
    activeTaskId: number;
    showTaskLength: number;
    canvasInfoListener: Subject<XmppTask[]>;
    hideTasksIds: any[];
    taskHeight: number;
    startTaskIndex: number;
    showTask: Array<XmppTask>;
    maxUID: any;
    deleteTasksSqlIdStore: string[];
    /** 勾选的所有任务 */
    selectedTasks: XmppTask[];
    updateTaskHandle(param: IXmpp): void;
    nextDatePipe(startDate: any): any;
  };
  calendar: {
    calendarId: string;
    weeksArry: any[];
    calenderWidth: number;
    baseCellWidth: number;
    weekDays: XmppWeekDay[];
    exceptDate: XmppExceptDate[];
    pauseWeekDayTypes: number[];
    pauseWeekDays: string[];
    minLineDay: any;
    maxLineDay: any;
  };
  draw: {
    ctxMask: CanvasRenderingContext2D;
    ctxRatio: number;
    selectedTaskId: number;
    canvasInfo: any[];
    exceptCanvasInfo: any[];
    canvasHeight: number;
    actualCanvasInfo: any[];
    canvasWidth: number;
    baseZoom: number;
    color: {
      /** 计划时间颜色 */
      planColor: string;
      /** 任务为关键线路时，计划时间颜色 */
      planKeyColor: string;
      /** 实际时间颜色 */
      Actualcolor: string;
      /** 任务为关键线路时，实际时间颜色 */
      ActualkeyColor: string;
      /** 箭头及连线颜色 */
      arrowColor: string;
      /** 任务为关键线路时，箭头及连线颜色 */
      arrowKeyColor: string;
      /** 例外日期区域的颜色 */
      exceptDateColor: string;
    };
    lineHeight: number;
    actualLineHeight: number;
    canvasLeftHide: number;
  };
  column: {
    columnNames: any[];
    totalWidth: number;
    setColumn(param: XmppColumn[]): void;
  };
  render(): void;
  addGanttEventListener(type: string, cb?: (res: any) => void): void;
}

export interface IMPPProject {
  actualsInSync: boolean;
  adminProject: boolean;
  assignments: any[];
  assignmentsUUIDs: string;
  author: string;
  autoAddNewResourcesAndTasks: boolean;
  autolink: boolean;
  baselineForEarnedValue: number;
  calendarUID: number;
  calendars: IMppCalendar[];
  calendarsUUIDs: string;
  category: string;
  company: string;
  creationDate: string;
  criticalSlackLimit: number;
  currencyCode: string;
  currencyDigits: number;
  currencySymbol: string;
  currencySymbolPosition: number;
  currentDate: string;
  daysPerMonth: number;
  defaultFinishTime: string;
  defaultFixedCostAccrual: number;
  defaultOvertimeRate: number;
  defaultStandardRate: number;
  defaultStartTime: string;
  defaultTaskEVMethod: number;
  defaultTaskType: number;
  durationFormat: number;
  earnedValueMethod: number;
  editableActualCosts: boolean;
  enabled: boolean;
  extendedAttributes: any[];
  extendedAttributesUUIDs: string;
  extendedCreationDate: string;
  finishDate: string;
  fiscalYearStart: boolean;
  fyStartDate: number;
  honorConstraints: boolean;
  id: string;
  insertedProjectsLikeSummary: boolean;
  lastSaved: string;
  manager: string;
  microsoftProjectServerURL: boolean;
  minutesPerDay: number;
  minutesPerWeek: number;
  moveCompletedEndsBack: boolean;
  moveCompletedEndsForward: boolean;
  moveRemainingStartsBack: boolean;
  moveRemainingStartsForward: boolean;
  multipleCriticalPaths: boolean;
  name: string;
  newTaskStartDate: number;
  newTasksEffortDriven: boolean;
  newTasksEstimated: boolean;
  parentId: string;
  projectExternallyEdited: boolean;
  removeFileProperties: boolean;
  resources: XmppResource[];
  resourcesUUIDs: string;
  revision: number;
  saveVersion: number;
  scheduleFromStart: boolean;
  splitsInProgressTasks: boolean;
  spreadActualCost: boolean;
  spreadPercentComplete: boolean;
  startDate: string;
  statusDate: string;
  subject: string;
  taskUpdatesResource: boolean;
  tasks: IMppTask[];
  tasksUUIDs: string;
  title: string;
  uid: string;
  weekStartDay: number;
  workFormat: number;
  [key: string]: any;
}

export interface IMppCalendar {
  uid: number;
  name: string;
  isBaseCalendar: boolean;
  baseCalendarUID: number;
  weekDays: IMppWeekDay[];
  weekDaysUUIDs: string;
  exceptions: IMppCalendarException[];
  exceptionsUUIDs: string;
  enabled: boolean;
  parentId: string;
  id: string;
}

export interface IMppWeekDay {
  dayType: number;
  dayWorking: boolean;
  fromDate: string;
  toDate: string;
  fromTime_0: string;
  toTime_0: string;
  fromTime_1: string;
  toTime_1: string;
  fromTime_2: string;
  toTime_2: string;
  fromTime_3: string;
  toTime_3: string;
  fromTime_4: string;
  toTime_4: string;
  enabled: boolean;
  parentId: string;
  id: string;
}

export interface IMppCalendarException {
  enteredByOccurrences: boolean;
  fromDate: string;
  toDate: string;
  occurrences: number;
  name: string;
  type: number;
  period: number;
  dasyOfWeek: number;
  monthIten: number;
  monthPosition: number;
  month: number;
  monthDay: number;
  dayWorking: boolean;
  parentId: string;
  id: string;
}

export interface IMppTask {
  active: number;
  actualCost: number;
  actualDuration: any;
  actualFinish: any;
  actualOvertimeCost: number;
  actualOvertimeWork: any;
  actualOvertimeWorkProtected: any;
  actualStart: any;
  actualWork: any;
  actualWorkProtected: any;
  acwp: number;
  baseline: any[];
  baselineUUIDs: any;
  bcwp: number;
  bcws: number;
  calendarUID: number;
  commitmentFinish: any;
  commitmentStart: any;
  commitmentType: number;
  constraintDate: any;
  constraintType: number;
  contact: any;
  cost: number;
  createDate: string;
  critical: boolean;
  cv: number;
  deadline: any;
  duration: string;
  durationFormat: number;
  earlyFinish: any;
  earlyStart: any;
  earnedValueMethod: number;
  effortDriven: boolean;
  enabled: boolean;
  estimated: boolean;
  extendedAttribute: any[];
  extendedAttributeUUIDs: any;
  externalTask: boolean;
  externalTaskProject: any;
  finish: string;
  finishVariance: number;
  fixedCost: number;
  fixedCostAccrual: any;
  freeSlack: number;
  hideBar: boolean;
  hyperlink: any;
  hyperlinkAddress: any;
  hyperlinkSubAddress: any;
  id: string;
  ignoreResourceCalendar: boolean;
  isNull: boolean;
  isPublished: boolean;
  isSubproject: boolean;
  isSubprojectReadOnly: boolean;
  lateFinish: any;
  lateStart: any;
  levelAssignments: boolean;
  levelingCanSplit: boolean;
  levelingDelay: number;
  levelingDelayFormat: number;
  manual: number;
  isMilepost: boolean;
  // milestone: boolean;
  name: string;
  notes: any;
  outlineLevel: number;
  outlineNumber: any;
  overAllocated: boolean;
  overtimeCost: number;
  overtimeWork: any;
  parentId: string;
  percentComplete: number;
  percentWorkComplete: number;
  physicalPercentComplete: number;
  preLeveledFinish: any;
  preLeveledStart: any;
  predecessorLink: any[];
  predecessorLinkUUIDs: any;
  priority: number;
  recurring: boolean;
  regularWork: any;
  remainingCost: number;
  remainingDuration: any;
  remainingOvertimeCost: number;
  remainingOvertimeWork: any;
  remainingWork: any;
  resume: any;
  resumeValid: boolean;
  rollup: boolean;
  starVariance: number;
  start: string;
  statusmanager: any;
  stop: any;
  subprojectName: any;
  summary: boolean;
  totalSlack: number;
  type: number;
  uid: number;
  wbs: string;
  wbsLevel: any;
  work: any;
  workVariance: number;
  _ID: number;
  // 自定义参数
  customAttrs: string;
}

export interface IMppExtendAttr {
  alias: any;
  appendNewValues: boolean;
  autoRollDown: boolean;
  calculationType: number;
  cfType: number;
  default: any;
  defaultGuid: any;
  elemType: number;
  enabled: boolean;
  fieldID: string;
  fieldName: string;
  formula: any;
  id: string;
  ltuid: any;
  maxMultiValues: number;
  parentId: string;
  phoneticAlias: any;
  restrictValues: boolean;
  rollupType: number;
  secondaryPID: any;
  userDef: boolean;
  valuelistSortOrder: number;
  _Guid: any;
}

export interface XmppTaskExtendedAttribute {
  fieldID: string;
  value: string;
  valueGUID: string;
  durationFormat: number;
  enabled: boolean;
  parentId: string;
  id: string;
}

export interface XmppResource {
  id?: string;
  uid: number;
  name: string;
  start: string;
  finish: string;
  work?: string;
  parentId: string;
}

export interface XmppAssignment {
  resourceUID: number;
  start: string;
  finish: string;
  id?: string;
  parentId: string;
  resourceName: string;
  uid?: number;
  taskUID: number;
}

export interface XmppWeekDay {
  id: string;
  dayType: number;
  dayWorking: boolean;
  fromDate: string;
  toDate: string;
  dayText: string;
}

export interface XmppExceptDate {
  id?: string;
  name: string;
  fromDate: string;
  toDate: string;
  parentId?: string;
}

export class XmppTask {
  /**
   * 公用参数
   */
  extendedAttribute: XmppTaskExtendedAttribute[];
  customExtendAttr?: XmppTaskExtendedAttribute;
  id?: number;
  public symbol?: number;
  public sqId?: string;
  public uid?: number;
  public wbs?: string | any;
  public taskName?: string;
  public isSelected = false;
  public taskLevel: string | any; // 任务等级
  public step = 0;
  public childTaskID?: any[] = [];
  public parentTaskID?: number = null;
  public level = 1;
  public isFold = false;
  public ganttChartId?: string;
  public defaultData = null;
  public bindings = [];
  public allFinished = false;
  public isMilepost = false;

  public _prevRelation?: XmppPredecessorLink[];
  public get prevRelation() {
    return this._prevRelation;
  }
  public set prevRelation(value) {
    const newRelations = [];
    if (value.length > 0) {
      value.forEach((pr) => {
        let str = pr.prevId + prevType2Str[pr.relation];
        if (pr.delay !== 0) {
          str = str + '+' + pr.delay;
        }
        newRelations.push(str);
      });
    } else {
      this.prevRelationStr = '';
    }
    this.prevRelationStr = newRelations.join(',');
    this._prevRelation = value;
  }

  public prevRelationStr: string;

  private _exceptDuration = 0;
  public get exceptDuration() {
    return this._exceptDuration;
  }

  public set exceptDuration(value) {
    this._exceptDuration = value;
  }

  // private _exceptActualDuration = 0;
  // public get exceptActualDuration() {
  //     return this._exceptActualDuration;
  // }

  // public set exceptActualDuration(value) {
  //     // 实际时间
  //     if (this._actualDuration) {
  //         this.showActualDuration = this._actualDuration - value;
  //     } else {
  //         if (this._actualStartDate && this._actualEndDate) {
  //             const num = moment(this._actualEndDate).clone().diff(moment(this._actualStartDate), 'days');
  //             this.showActualDuration = num + 1;
  //         }
  //     }
  //     this._exceptActualDuration = value;
  // }

  /**
   * 计划参数
   */
  public truePrevTaskID?: any;
  public isKey = false;
  public laterChildId?: number = null;
  public earlierChildId?: number = null;
  private _startDate = null;
  public get startDate() {
    if (this._startDate) {
      return moment(this._startDate).format('YYYY-MM-DD');
    } else {
      return null;
    }
  }
  public set startDate(value) {
    // 不判断例外日期
    // if (!this.firstSet && this.childTaskID.length <= 0 && value) {
    //     if (this._duration !== 0) {
    //         this._endDate = moment(value).add(this._duration - 1, 'days').toDate();
    //     } else {
    //         this._endDate = value;
    //     }
    // }

    this._startDate = value;
  }

  public parentTaskStore: string;

  private _duration;
  public get duration() {
    return !isNullOrUndefined(this._duration) ? this._duration : 0;
  }
  public set duration(value) {
    // if (!this.firstSet && this.childTaskID.length === 0 && !isNullOrUndefined(value)) {
    //     value = this.handleDuration(parseInt(value));
    // }
    this._duration = value;
  }
  private _endDate = null;
  public get endDate() {
    if (this._endDate) {
      return moment(this._endDate).format('YYYY-MM-DD');
    } else {
      return null;
    }
  }
  public set endDate(value) {
    this._endDate = value;
  }

  /**
   * 实际参数
   */
  public actulaTruePrevTaskID?: any;
  public isActualKey = false;
  public acLaterChildId = null;
  public acEarlierChildId = null;

  /** 实际开始时间 */
  private _actualStartDate = null;
  public get actualStartDate() {
    if (this._actualStartDate) {
      return moment(this._actualStartDate).format('YYYY-MM-DD');
    } else {
      return null;
    }
  }
  public set actualStartDate(value) {
    this._actualStartDate = value;
  }

  /** 实际工期 */
  private _actualDuration?: any = null;
  public get actualDuration() {
    return !isNullOrUndefined(this._actualDuration) ? this._actualDuration : 0;
  }
  public set actualDuration(value) {
    this._actualDuration = value;
  }

  /** 实际结束时间 */
  private _actualEndDate?: any = null;
  public get actualEndDate() {
    if (this._actualEndDate) {
      return moment(this._actualEndDate).format('YYYY-MM-DD');
    } else {
      return null;
    }
  }
  public set actualEndDate(value) {
    this._actualEndDate = value;
  }

  tags?: {
    key: string;
    name?: string;
    iconUrl?: string;
    iconType?: string;
  }[] = [];
  CustomAttrs: string;
  Xmpp: any;
  public _assignments: XmppAssignment[] = [];
  public get assignments() {
    return this._assignments;
  }
  public set assignments(value) {
    const assignments = [];
    for (const rs of value) {
      assignments.push(rs.resourceName);
    }
    this.assignmentsStr = assignments.join(',');
    this._assignments = value;
  }
  public assignmentsStr: string;
  public finishRate: string = null;

  [key: string]: any;
  firstSet = false;

  public constructor(param: any) {
    this.firstSet = true;
    param.Xmpp && (this.Xmpp = param.Xmpp);
    // 任务自定义的属性集合
    param.CustomAttrs && (this.CustomAttrs = param.CustomAttrs);
    // 任务所有的extendedAttribute
    param.extendedAttribute &&
      (this.extendedAttribute = param.extendedAttribute);
    // xmpp自定义extendedAttribute，即FildId为'188744016'的
    param.customExtendAttr && (this.customExtendAttr = param.customExtendAttr);
    // 数据库id
    param.sqId && (this.sqId = param.sqId);
    // UID
    param.uid && (this.uid = param.uid);
    // 标识符--orderId
    param.id && (this.id = param.id);
    // 标识符--对接模型
    param.symbol && (this.symbol = param.symbol);
    // 所属甘特图id
    param.ganttChartId && (this.ganttChartId = param.ganttChartId);
    // 所属甘特图id
    param.wbs && (this.wbs = param.wbs);
    // 任务名称
    !isNullOrUndefined(param.taskName) && (this.taskName = param.taskName);
    // 是否被选中
    param.isSelected && (this.isSelected = param.isSelected);
    // 计划工期
    !isNullOrUndefined(param.duration) && (this.duration = param.duration);
    // 计划开始时间
    param.startDate && (this.startDate = param.startDate);
    // 计划完成时间
    param.endDate && (this.endDate = param.endDate);
    // 实际工期
    !isNullOrUndefined(param.actualDuration) &&
      (this.actualDuration = param.actualDuration);
    // 实际开始时间
    param.actualStartDate && (this.actualStartDate = param.actualStartDate);
    // 实际完成时间
    param.actualEndDate && (this.actualEndDate = param.actualEndDate);
    // 紧前任务
    param.truePrevTaskID && (this.truePrevTaskID = param.truePrevTaskID);
    // 是否是关键任务
    param.isKey && (this.isKey = param.isKey);
    // 子任务id
    param.childTaskID && (this.childTaskID = param.childTaskID);
    // 父任务id
    param.parentTaskID && (this.parentTaskID = param.parentTaskID);
    // 级别
    param.level && (this.level = param.level);
    // 绑定的构件
    param.bindings && (this.bindings = param.bindings);
    // 是否是里程碑
    param.isMilepost && (this.isMilepost = param.isMilepost);
    // 完成率
    param.finishRate && (this.finishRate = param.finishRate);
    // 任务等级
    param.taskLevel && (this.taskLevel = param.taskLevel);
    // 前置任务
    param.prevRelation && (this.prevRelation = param.prevRelation);
    // 目前的状态
    param.step && (this.step = param.step);
    // 摘要任务下,影响摘要任务结束时间时间的任务
    param.laterChildId && (this.laterChildId = param.laterChildId);
    // 摘要任务下,影响摘要任务开始时间的任务
    param.earlierChildId && (this.earlierChildId = param.earlierChildId);
    // 摘要任务下,影响摘要任务结束时间时间的任务
    param.acLaterChildId && (this.acLaterChildId = param.acLaterChildId);
    // 摘要任务下,影响摘要任务开始时间的任务
    param.acEarlierChildId && (this.acEarlierChildId = param.acEarlierChildId);
    // 任务中的额外日期工期
    param.exceptDuration && (this.exceptDuration = param.exceptDuration);
    // 任务中的额外日期工期
    // (param.exceptActualDuration) && (this.exceptActualDuration = param.exceptActualDuration);
    param.tags && (this.tags = param.tags);
    param.assignments && (this.assignments = param.assignments);

    const defaultData = {
      startDate: this.startDate,
      duration: this.duration,
      endDate: this.endDate,
      taskName: this.taskName,
      parentTaskID: this.parentTaskID,
      actualDuration: this.actualDuration,
      actualStartDate: this.actualStartDate,
      actualEndDate: this.actualEndDate,
      level: this.level,
      wbs: this.wbs,
      tags: this.tags,
      assignments: this.assignments,
      prevRelation: this.prevRelation,
      finishRate: this.finishRate,
      isMilepost: this.isMilepost,
      taskLevel: this.taskLevel,
    };
    this.firstSet = false;
    this.defaultData = JSON.parse(JSON.stringify(defaultData));
  }

  public toJson() {
    return {
      orderId: this.id,
      ganttChartId: this.ganttChartId,
      taskName: this.taskName,
      startAt: this.startDate ? moment(this.startDate).unix() : null,
      finishAt: this.endDate ? moment(this.endDate).unix() : null,
      duration: this.duration,
      actualStartAt: this.actualStartDate
        ? moment(this.actualStartDate).unix()
        : null,
      actualFinishAt: this.actualEndDate
        ? moment(this.actualEndDate).unix()
        : null,
      parentTaskId: this.parentTaskID,
      preTask: JSON.stringify(this.prevRelation),
      childTaskID: this.childTaskID,
      level: this.level,
      bindings: this.bindings,
      isMilepost: this.isMilepost,
      // milestone: this.isMilepost ? 1 : 0,
      finishRate: this.finishRate,
      step: this.step,
      // sqlStartDate: this.sqlStartDate ? moment(this.sqlStartDate).unix() : null,
      // sqlEndDate: this.sqlEndDate ? moment(this.sqlEndDate).unix() : null,
      // sqlDuration: this.sqlDuration,
      // sqlActualStartDate: this.sqlActualStartDate,
      // sqlActualEndDate: this.sqlActualEndDate,
      // sqlActualDuration: this.sqlActualDuration,
    };
  }

  public toEditJson() {
    return {
      id: this.sqId,
      orderId: this.id,
      ganttChartId: this.ganttChartId,
      taskName: this.taskName,
      startAt: this.startDate ? moment(this.startDate).unix() : null,
      finishAt: this.endDate ? moment(this.endDate).unix() : null,
      duration: this.duration,
      actualStartAt: this.actualStartDate
        ? moment(this.actualStartDate).unix()
        : null,
      actualFinishAt: this.actualEndDate
        ? moment(this.actualEndDate).unix()
        : null,
      parentTaskId: this.parentTaskID,
      preTask: JSON.stringify(this.prevRelation),
      childTaskID: this.childTaskID,
      level: this.level,
      bindings: this.bindings,
      isMilepost: this.isMilepost,
      // milestone: this.isMilepost ? 1 : 0,
      finishRate: this.finishRate,
      step: this.step,
      // sqlStartDate: this.sqlStartDate ? moment(this.sqlStartDate).unix() : null,
      // sqlEndDate: this.sqlEndDate ? moment(this.sqlEndDate).unix() : null,
      // sqlDuration: this.sqlDuration,
      // sqlActualStartDate: this.sqlActualStartDate ? moment(this.sqlActualStartDate).unix() : null,
      // sqlActualEndDate: this.sqlActualEndDate ? moment(this.sqlActualEndDate).unix() : null,
      // sqlActualDuration: this.sqlActualDuration ? moment(this.sqlActualDuration).unix() : null,
    };
  }

  public toMpp() {
    const CustomAttrs: any = new Object();
    CustomAttrs.tags = this.tags;
    CustomAttrs.finishRate = this.finishRate;
    CustomAttrs.taskLevel = this.taskLevel;
    return {
      Id: this.sqId, // 数据库id
      _ID: this.id, // orderid
      UID: this.uid,
      ParentId: this.ganttChartId,
      Name: this.taskName,
      Start: this.startDate
        ? moment(this.startDate).format('YYYY-MM-DD')
        : null,
      Finish: this.endDate ? moment(this.endDate).format('YYYY-MM-DD') : null,
      Duration: `PT${this.duration * 8}H0M0S`,
      ActualStart: this.actualStartDate
        ? moment(this.actualStartDate).format('YYYY-MM-DD')
        : null,
      ActualFinish: this.actualEndDate
        ? moment(this.actualEndDate).format('YYYY-MM-DD')
        : null,
      WBS: this.wbs,
      IsMilepost: this.isMilepost,
      CustomAttrs: JSON.stringify(CustomAttrs),
    };
  }

  getMomentStart(date) {
    const format = moment(date).format('YYYY-MM-DD');
    return moment(format).format();
  }

  public handleStartDate(startDate) {
    startDate = this.getMomentStart(startDate);
    const duration = this._duration;
    let correctDate = startDate;
    // if (this.exceptDate && this.exceptDate.length > 0) {
    //     this.exceptDate.forEach((item) => {
    //         if (unix <= item.endDate && unix >= item.startDate) {
    //             // 如果所选s在额外日期之间，d不变，e=s+d
    //             // let errorDate = moment(startDate).format('MM/DD');
    //             const correct = moment.unix(item.endDate).clone().add(1, 'days').toDate();
    //             // GanttModel.message.confirm('warning', `任务${this.id}的计划开始时间${errorDate}为非工作日，将移动该任务开始时间到下一工作日${correct.format('MM/DD')}。`, 10000);
    //             startDate = correct;
    //         }
    //     });
    // }
    const getStartDateFromWeekDay = (startDateInput) => {
      const dayWeekType = new Date(
        moment(startDateInput).unix() * 1000
      ).getDay();
      if (
        this.Xmpp.calendar.pauseWeekDayTypes.indexOf(dayWeekType + 1) !== -1
      ) {
        const nextDate = moment(startDateInput).clone().add(1, 'days').toDate();
        startDateInput = nextDate;
        getStartDateFromWeekDay(startDateInput);
      } else {
        correctDate = startDateInput;
        return;
      }
    };
    const startUnix = startDate ? moment(startDate).unix() : null;
    if (this.Xmpp.calendar.exceptDate) {
      this.Xmpp.calendar.exceptDate.forEach((exceptDate) => {
        // 任务开始时间在例外日期之间
        const exceptDateStart = moment(exceptDate.fromDate).unix();
        const exceptDateEnd = moment(exceptDate.toDate).unix();
        if (startUnix <= exceptDateEnd && startUnix >= exceptDateStart) {
          const nextDate = moment
            .unix(exceptDateEnd)
            .clone()
            .add(1, 'days')
            .toDate();
          if (moment(nextDate).clone().isAfter(moment(startDate))) {
            correctDate = correctDate;
            // maxStartDate = correctDate;
          }
        }
        // // 任务结束时间在例外日期之间
        // if (endUnix <= exceptDateEnd && endUnix >= exceptDateStart) {
        //     const correctDate = moment.unix(exceptDateEnd).clone().add(1, 'days').toDate();
        //     task.endDate = correctDate;
        // }
      });
    }
    getStartDateFromWeekDay(startDate);
    startDate = correctDate;

    return startDate;
  }

  public handleEndDate(endDate) {
    endDate = this.getMomentStart(endDate);
    // const unix = endDate ? moment(endDate).unix() : null;
    const duration = this._duration;
    let correctDate = endDate;
    // const startUnix = moment(this._startDate).unix();
    // if (this.exceptDate && this.exceptDate.length > 0) {
    //     // 有额外日期
    //     this.exceptDate.forEach((item) => {
    //         if (unix <= item.endDate && unix >= item.startDate) {
    //             // 所选endDate在额外日期之间，e自动延至额外日期结束后一天，s不变，d=e-s
    //             const errorDate = moment(endDate).format('MM/DD');
    //             const correct = moment.unix(item.endDate).clone().add(1, 'days');
    //             // GanttModel.message.confirm('warning', `任务${this.id}的计划结束时间${errorDate}为非工作日，将移动该任务结束时间到下一工作日${correct.format('MM/DD')}。`, 10000);
    //             this._exceptDuration = moment.unix(item.endDate).clone().diff(moment.unix(item.startDate), 'days') + 1;
    //             endDate = correct;
    //         }
    //     });
    // }
    const getEndDateFromWeekDay = (dateInput) => {
      const dayWeekType = new Date(moment(dateInput).unix() * 1000).getDay();
      if (
        this.Xmpp.calendar.pauseWeekDayTypes.indexOf(dayWeekType + 1) !== -1
      ) {
        const nextDate = moment(dateInput).clone().add(1, 'days').toDate();
        dateInput = nextDate;
        getEndDateFromWeekDay(dateInput);
      } else {
        correctDate = dateInput;
        return;
      }
    };
    if (this.Xmpp.calendar.exceptDate) {
      const endUnix = endDate ? moment(endDate).unix() : null;
      this.Xmpp.calendar.exceptDate.forEach((exceptDate) => {
        const exceptDateStart = moment(exceptDate.fromDate).unix();
        const exceptDateEnd = moment(exceptDate.toDate).unix();
        // 任务结束时间在例外日期之间
        if (endUnix <= exceptDateEnd && endUnix >= exceptDateStart) {
          const nextDate = moment
            .unix(exceptDateEnd)
            .clone()
            .add(1, 'days')
            .toDate();
          correctDate = nextDate;
        }
      });
    }

    getEndDateFromWeekDay(endDate);
    endDate = correctDate;
    // if (!startDate) {
    //     if (duration && duration > 0) {
    //         // 没s，有d
    //         this._startDate = moment(endDate).clone().subtract(duration - 1, 'days').toDate();
    //     } else {
    //         // 没s，没d
    //         this._startDate = endDate;
    //     }
    // } else {
    //     if (moment(endDate).isBefore(moment(startDate))) {
    //         this._startDate = moment(endDate).subtract(duration - 1, 'days').toDate();
    //     }
    // }

    return endDate;
  }

  public handleDuration(duration) {
    if (this._startDate && this._endDate) {
      if (duration) {
        const endDate = moment(this._startDate)
          .add(duration - 1, 'days')
          .toDate();
        this._endDate = endDate;
      } else if (duration === 0) {
        this._endDate = this._startDate;
      }
    }
    return duration;
  }

  // public handleActualStartDate(startDate) {
  //     const unix = startDate ? moment(startDate).unix() : null;
  //     const actualEndDate = this._actualEndDate;
  //     const duration = this._actualDuration;
  //     if (this.exceptDate && this.exceptDate.length > 0) {
  //         this.exceptDate.forEach((item) => {
  //             if (unix <= item.endDate && unix >= item.startDate) {
  //                 // 如果所选s在额外日期之间，d不变，e=s+d
  //                 const errorDate = moment(startDate).format('MM/DD');
  //                 const correct = moment.unix(item.endDate).clone().add(1, 'days').toDate();
  //                 // GanttModel.message.confirm('warning', `任务${this.id}的计划开始时间${errorDate}为非工作日，将移动该任务开始时间到下一工作日${correct.format('MM/DD')}。`, 10000);
  //                 startDate = correct;
  //             }
  //         });
  //     }
  //     if (!actualEndDate) {
  //         if (duration && duration > 0) {
  //             // 没有endDate,有duration
  //             this.actualEndDate = moment(startDate).add(duration - 1, 'days').toDate();
  //         } else {
  //             // 没有duration
  //             this._actualEndDate = startDate;
  //         }
  //     } else {
  //         this.actualEndDate = moment(startDate).add(duration - 1, 'days').toDate();
  //     }
  //     return startDate;
  // }

  public handleActualEndDate(endDate) {
    const unix = endDate ? moment(endDate).unix() : null;
    const startDate = this._actualStartDate;
    const duration = this._actualDuration;
    const startUnix = moment(this._actualStartDate).unix();
    if (this.exceptDate && this.exceptDate.length > 0) {
      // 有额外日期
      this.exceptDate.forEach((item) => {
        if (unix <= item.endDate && unix >= item.startDate) {
          // 所选endDate在额外日期之间，e自动延至额外日期结束后一天，s不变，d=e-s
          const errorDate = moment(endDate).format('MM/DD');
          const correct = moment
            .unix(item.endDate)
            .clone()
            .add(1, 'days')
            .toDate();
          // GanttModel.message.confirm('warning', `任务${this.id}的计划结束时间${errorDate}为非工作日，将移动该任务结束时间到下一工作日${correct.format('MM/DD')}。`, 10000);
          this._exceptDuration =
            moment
              .unix(item.endDate)
              .clone()
              .diff(moment.unix(item.startDate), 'days') + 1;
          endDate = correct;
        }
      });
    }
    if (!startDate) {
      if (duration && duration > 0) {
        // 没s，有d
        this._actualStartDate = moment(endDate)
          .clone()
          .subtract(duration - 1, 'days')
          .toDate();
      } else {
        // 没s，没d
        this._actualStartDate = endDate;
      }
    } else {
      if (moment(endDate).isBefore(moment(startDate))) {
        this._actualStartDate = moment(endDate)
          .subtract(duration - 1, 'days')
          .toDate();
      } else {
        this._actualDuration =
          moment(endDate).clone().diff(moment(startDate), 'days') + 1;
      }
    }
    return endDate;
  }

  public handleActualDuration(duration) {
    if (this._actualStartDate && this._actualEndDate) {
      if (duration) {
        const endDate = moment(this._actualStartDate)
          .add(duration - 1, 'days')
          .toDate();
        this._actualEndDate = endDate;
      } else if (duration === 0) {
        this._actualEndDate = this._actualStartDate;
      }
    }
    return duration;
  }
}

export class XmppPredecessorLink {
  public id?: string;
  public prevId?: any;
  public relation: number;
  public delay = 0;
  public predecessorLink?: number;
  public defaultPerv?: any;
  public isDelete = 0;
  public parentId: string;
  public constructor(param) {
    param.id && (this.id = param.id);
    param.prevId && (this.prevId = param.prevId);
    !isNullOrUndefined(param.relation) && (this.relation = param.relation);
    param.delay && (this.delay = param.delay);
    param.predecessorLink && (this.predecessorLink = param.predecessorLink);
    param.defaultPerv && (this.defaultPerv = param.defaultPerv);
    param.isDelete && (this.isDelete = param.isDelete);
    param.parentId && (this.parentId = param.parentId);
  }
}
