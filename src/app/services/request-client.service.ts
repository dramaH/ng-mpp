import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '../../../node_modules/@angular/common/http';


@Injectable({
  providedIn: 'root'
})
export class RequestClientService {

  constructor(
    private http: HttpClient
    // private loginService: LoginService,
    // public jwtHelper: JwtHelperService
  ) { }
  public get(url: string, params, header?): Promise<any> {
    let headers = this.creatHeader(header)
    return this.http.get(url, { headers: headers, params: params })
      .toPromise().then(this.checkResponeData).catch(this.throwError)
  }
  public post(url: string, params?, header?): Promise<any> {
    let headers = this.creatHeader(header)
    return this.http.post(url, params, { headers: headers })
      .toPromise().then(this.checkResponeData).catch(this.throwError);
  }
  public put(url: string, params, header?): Promise<any> {
    let headers = this.creatHeader(header)
    return this.http.put(url, params, { headers: headers })
      .toPromise().then(this.checkResponeData).catch(this.throwError);
  }
  public delete(url: string, params, header?): Promise<any> {
    let headers = this.creatHeader(header)
    return this.http.delete(url, { headers: headers, params: params })
      .toPromise().then(this.checkResponeData).catch(this.throwError);
  }

  public checkResponeData(res) {
    return res;
  }

  public async throwError(error) {
    console.error(error);
    return error.error;
  }

  public creatHeader(header?) {
    let formObj = {}
    for (let key in header) {
      formObj[key] = header[key];
    }
    return new HttpHeaders(formObj);
  }

  public getAPDToken() {
    if (!window.localStorage.getItem('APDInfo')) return 'Bearer ' + null;
    const apdInfo = JSON.parse(window.localStorage.getItem('APDInfo'));
    return 'Bearer ' + apdInfo.accessToken;
  }

  // public getSSO(url: string, params?, header?): Promise<any> {
  //   let headers = this.creatSSOHeader(header)
  //   return this.http.get(url, { headers: headers, params: params })
  //     .toPromise().then(this.checkResponeData).catch(this.throwError)
  // }
  // public postSSO(url: string, params, header?): Promise<any> {
  //   let headers = this.creatSSOHeader(header)
  //   return this.http.post(url, params, { headers: headers })
  //     .toPromise().then(this.checkResponeData).catch(this.throwError);
  // }
  // public putSSO(url: string, params, header?): Promise<any> {
  //   let headers = this.creatSSOHeader(header)
  //   return this.http.put(url, params, { headers: headers })
  //     .toPromise().then(this.checkResponeData).catch(this.throwError);
  // }
  // public deleteSSO(url: string, params, header?): Promise<any> {
  //   let headers = this.creatSSOHeader(header)
  //   return this.http.delete(url, { headers: headers, params: params })
  //     .toPromise().then(this.checkResponeData).catch(this.throwError);
  // }

  public creatSSOHeader(header?) {
    let formObj = {
      'Authorization': this.getSSOToken()
    }
    for (let key in header) {
      formObj[key] = header[key];
    }
    return new HttpHeaders(formObj);
  }

  public getSSOToken() {
    return 'Bearer ' + sessionStorage.getItem('access_token');
  }

}
