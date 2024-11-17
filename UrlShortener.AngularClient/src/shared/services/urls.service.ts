import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {CreateUrlRequest} from "../requests/CreateUrlRequest";
import {ApiResponse} from "../responses/ApiResponse";
import {environment} from "../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class UrlsService {

  constructor(private http: HttpClient) { }

  public getUrls(): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${environment.backendUrl}/api/urls/get-urls`);
  }

  public createUrl(req: CreateUrlRequest): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${environment.backendUrl}/api/urls/create-url`, req);
  }

  public deleteUrl(url: string): Observable<ApiResponse> {
    return this.http.delete<ApiResponse>(`${environment.backendUrl}/api/urls/delete-url/${url}`);
  }

  public deleteAllUrls(): Observable<ApiResponse> {
    return this.http.delete<ApiResponse>(`${environment.backendUrl}/api/urls/delete-all-urls`);
  }
}
