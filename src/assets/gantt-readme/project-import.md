### **导入project项目**
初始话项目需要传递
#### **相关接口**
```js
// 导入*.mpp文件并创建项目
api:`/mpp/api/v1/mpp/import/${parentId}`  
param:{
  file:File
}

// 导入*.mpp文件获取taskList不创建项目
api:`/mpp/api/v1/mpp/importWidthoutSave/${parentId}`  
param:{
  file:File
}
```

#### **示例**：
```js
  const formdata = new FormData();
  formdata.set('file', uploadForm.uploadMMP);
  const res = await this.requestClientService.postSSO(GANTTURL + `/mpp/api/v1/mpp/import/${projectId}`, formdata);
```