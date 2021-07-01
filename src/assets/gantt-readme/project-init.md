### **初始化项目**
从mpp服务加载项目需要通过projectId（进度项目id）来请求mpp service的接口，需要完成以下操作：  
1、获取项目信息、资源属性和任务列表  
2、设计进度列信息，可自定义，参照IXmppOptions  
3、设计甘特图横道线颜色，可自定义，参照IXmppOptions  
4、一切准备就绪，初始化项目，获取该项目gantt组件，以及全局变量Xmpp  
```js
// component组件
this.Gantt = Project.newProject(this.container, this.option);
// 全局变量Xmpp
this.Xmpp = this.Gantt.Xmpp;
```

#### **相关接口**

```js
// 获取项目信息  
接口:/mpp/api/v1/mpp/project/${ganttId}    
param:{}

// 获取项目资源属性  
接口:/mpp/api/v1/mpp/project/${ganttId}/extendedattributes    
param:{}

// 获取项目任务列表列表  
接口:/mpp/api/v1/mpp/project/${ganttId}/tasks  
param:{}
```

#### **示例**：
```js
  public async initProject(ganttId: string) {
    // 获取项目信息
    const ganttInfo: IMPPProject = await this.ganttRequestSev.getGanttInfo(ganttId);
    this.ganttInfo = ganttInfo;
    // 获取项目资源属性
    const extendAttrs = await this.ganttRequestSev.getGanttAttrs(ganttId);
    this.extendAttrs = extendAttrs;
    // 获取项目任务列表列表
    const taskRes = await this.ganttRequestSev.getTasksList(ganttId);
    this.tasks = taskRes;
    // 定义列
    let option: XmppOptions = {
      mppInfo: ganttInfo,
      mppExtendAttrs: extendAttrs,
      mppTasks: taskRes,
      columns: [
        {
          key: 'checkbox',
          width: 50,
          name: '选择',
          type: 'checkbox'
        },
        {
          key: 'tags',
          width: 100,
          name: '标签',
          type: 'tags',
          resize: true
        },
        {
          key: 'taskName',
          width: 100,
          name: '任务名称',
          type: 'taskName',
          resize: true
        },
        {
          key: 'startDate',
          width: 130,
          name: '计划开始时间',
          type: 'date'
        },
        {
          key: 'duration',
          width: 75,
          name: '工期'
        },
        {
          key: 'endDate',
          width: 130,
          name: '计划结束日期',
          type: 'date'
        },
        {
          key: 'actualStartDate',
          width: 130,
          name: '实际开始日期',
          type: 'date'
        },
        {
          key: 'actualDuration',
          width: 75,
          name: '实际工期'
        },
        {
          key: 'actualEndDate',
          width: 130,
          name: '实际结束日期',
          type: 'date'
        }
      ],
      color: {
        /** 计划时间颜色 */
        planColor: 'rgba(65,159,229, 1)',
        /** 任务为关键线路时，计划时间颜色 */
        planKeyColor: 'rgba(65,159,229, 1)',
        /** 任务为延期任务时，计划时间颜色 */
        planDelayColor: 'rgba(255, 128, 128, 1)',
        /** 实际时间颜色 */
        Actualcolor: '#419fe8',
        /** 任务为关键线路时，实际时间颜色 */
        ActualkeyColor: '#419fe8',
        /** 任务为延期任务时，实际时间颜色 */
        ActualDelayColor: '#ff8080',
        /** 箭头及连线颜色 */
        arrowColor: 'rgba(65,159,229, 1)',
        /** 任务为关键线路时，箭头及连线颜色 */
        arrowKeyColor: 'rgba(255, 128, 128, 1)',
        exceptDateColor: 'rgba(255, 128, 128, 1)'
      }
    };
    this.option = option;
    this.mpp = Project.newProject(this.container, this.option);
  }

```