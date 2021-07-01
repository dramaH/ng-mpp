import { Injectable, ComponentFactoryResolver, ViewContainerRef, ComponentFactory, ComponentRef } from '@angular/core';
import { RequestClientService } from '../../services/request-client.service';
import { IMppTask, IMppExtendAttr, XmppTaskExtendedAttribute, EXTENDATTRS, XmppWeekDay, XmppExceptDate } from 'projects/zzj/zzj-xmpp/src/public-api';
import { environment } from 'src/environments/environment';
const GANTTURL = environment.mppUrl;
// const GANTTURL = 'http://kzzbim.spddemo.com:88'
export const PROJECTID = 'bf894bfd-80ad-417d-bd31-a8af3200025c';
@Injectable({
  providedIn: 'root'
})
export class GanttRequestService {
  public constructor(
    private requestClientService: RequestClientService
  ) {
    // this.http = injector.get(Http);
    // this.httpClient = injector.get(HttpClient);
  }


  /**
   * 上传mpp文件，自定义项目title
   * @param uploadForm
   */
  public async uploadMMP(uploadForm): Promise<any> {
    const projectId = PROJECTID;
    // let projectId = this.modelService.modelId;
    const res = await this.requestClientService.post(GANTTURL + `/mpp/import`, uploadForm);
    if (res.success) {
      console.log(res)
      // console.log(res2);
      // res.item.title == res2.data.Title;
      return res.item;
    } else {
      return false;
    }
  }

  /**
   * 上传mpp文件，自定义项目title
   * @param uploadForm
   */
  public async uploadMMPXml(uploadForm): Promise<any> {
    const projectId = PROJECTID;
    // let projectId = this.modelService.modelId;
    const formdata = new FormData();
    formdata.set('file', uploadForm.uploadMMP);
    const res = await this.requestClientService.post(GANTTURL + `/mpp/importXml/${projectId}`, formdata);
    if (res.success) {
      const data = res.item;
      const id = res.item.id;
      const param = {
        Title: uploadForm.uploadTitle
      };
      const res2 = await this.updateGantt(id, param);
      // console.log(res2);
      // res.item.title == res2.data.Title;
      return res.item;
    } else {
      return false;
    }
  }


  public async uploadMppWithOutSave(uploadForm) {
    const projectId = PROJECTID;
    // let projectId = this.modelService.modelId;
    const formdata = new FormData();
    formdata.set('file', uploadForm.uploadMMP);
    const res = await this.requestClientService.post(GANTTURL + `/mpp/importWidthoutSave/${projectId}`, formdata);
    if (res.success) {
      const data = res.item;
      const id = res.item.id;
      const param = {
        Title: uploadForm.uploadTitle
      };
      const res2 = await this.updateGantt(id, param);
      // console.log(res2);
      // res.item.title == res2.data.Title;
      return res.item;
    } else {
      return false;
    }
  }

  public async updateGantt(id, param) {
    return this.requestClientService.put(GANTTURL + `/mpp/project/${id}`, JSON.stringify(param), { 'Content-Type': 'application/json' });
  }

  /**
   * 获取项目列表
   */
  public async getGanttList() {
    const projectId = PROJECTID;
    const res = await this.requestClientService.get(GANTTURL + `/mpp/${projectId}/projects`, {});
    if (res.success) {
      return res.items;
    } else {
      return [];
    }
  }

  /**
   * 删除gantt
   * @param ganttId
   */
  public async deleteGantt(ganttId) {
    await this.requestClientService.delete(GANTTURL + `/mpp/project/${ganttId}`, {});
  }

  public async updateTask(projectId: string, taskId: string, param) {
    const res = await this.requestClientService.put(GANTTURL + `/mpp/project/${projectId}/task/${taskId}`, param);
    return res;
  }

  /**
   * 获取task列表
   * @param ganttId
   */
  public async getTasksList(ganttId): Promise<Array<IMppTask>> {
    const res = await this.requestClientService.get(GANTTURL + `/mpp/project/${ganttId}/tasks`, {});
    if (res.success) {
      return res.items;
    } else {
      return [];
    }
  }

  /**
   * 获取进度信息
   * @param ganttId
   */
  public async getGanttInfo(ganttId: string) {
    const res = await this.requestClientService.get(GANTTURL + `/mpp/project/${ganttId}`, {});
    if (res.success) {
      return res.item;
    } else {
      return null;
    }
  }

  /**
   * 更新tasks（总）
   * @param ganttId
   * @param param
   */
  public async putTasks(ganttId: string, param: any): Promise<boolean> {
    const res = await this.requestClientService.put(GANTTURL + `/mpp/project/${ganttId}/batch`, param);
    if (res.success) {
      return true;
    } else {
      return false;
    }
  }

  /**
   * 新建进度
   * @param param
   */
  public async postGantt(param) {
    // let projectId = this.modelService.modelId;
    const projectId = PROJECTID;
    const res = await this.requestClientService.post(GANTTURL + `/mpp/project/${projectId}`, param);
    console.log(res);
    if (res.success) {
      return res.item.id;
    } else {
      return null;
    }

  }

  /**
   * 创建binding资源并关联project
   * @param ganttId
   */
  public async postGanttBindingAttr(ganttId: string) {
    const res = await this.requestClientService.post(GANTTURL + `/mpp/project/${ganttId}/extendedattribute`, EXTENDATTRS.binding);
    return res;
  }

  /**
   * 获取project所绑定的资源
   * @param ganttId
   */
  public async getGanttAttrs(ganttId: string): Promise<Array<IMppExtendAttr>> {
    const res = await this.requestClientService.get(GANTTURL + `/mpp/project/${ganttId}/extendedattributes`, {});
    return res.items;
  }

  /**
   * 获取task的拓展属性
   * @param ganttId
   * @param taskId
   */
  public async getTaskAttrs(ganttId: string, taskId: string): Promise<Array<any>> {
    const res = await this.requestClientService.get(GANTTURL + `/mpp/project/${ganttId}/task/${taskId}`, {});
    if (res.success) {
      return res.item.extendedAttribute;
    } else {
      return [];
    }
  }

  /**
   * 新增task绑定extend
   * @param ganttId
   * @param taskId
   * @param param
   */
  public async bindTaskExtendedAttribute(ganttId: string, taskId: string, param: { FieldID: string, Value: string }): Promise<XmppTaskExtendedAttribute> {
    // tslint:disable-next-line: max-line-length
    const res = await this.requestClientService.post(GANTTURL + `/mpp/project/${ganttId}/task/${taskId}/extendedattribute`, JSON.stringify(param), { 'Content-Type': 'application/json' });
    return res;
  }

  /**
   * 更新资源
   * @param ganttId 
   * @param taskId 
   * @param attrId 
   * @param param 
   */
  public async updateExtendedAttrbute(ganttId: string, taskId: string, attrId: string, param: { FieldID: string, Value: string }): Promise<XmppTaskExtendedAttribute> {
    // tslint:disable-next-line: max-line-length
    const res = await this.requestClientService.put(GANTTURL + `/mpp/project/${ganttId}/task/${taskId}/extendedattribute/${attrId}`, JSON.stringify(param), { 'Content-Type': 'application/json' });
    return res;
  }

  public async updateWeekDays(calendarId: string, weekDays: XmppWeekDay[]) {
    // tslint:disable-next-line: max-line-length
    const res = await this.requestClientService.put(GANTTURL + `/mpp/calendar/updateWeekDays/${calendarId}`, weekDays);
    return res;
  }

  public async updateException(input: XmppExceptDate[]) {
    // tslint:disable-next-line: max-line-length
    const res = await this.requestClientService.put(GANTTURL + `/mpp/calendar/updateException`, input);
    return res;
  }

  public async addException(input: XmppExceptDate[]) {
    // tslint:disable-next-line: max-line-length
    const res = await this.requestClientService.post(GANTTURL + `/mpp/calendar/addException`, input);
    return res;
  }

  public async deleteException(id: string) {
    // tslint:disable-next-line: max-line-length
    const res = await this.requestClientService.delete(GANTTURL + `/mpp/calendar/deleteException`, {id});
    return res;
  }

}
