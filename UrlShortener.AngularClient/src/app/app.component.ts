import {Component, OnInit} from '@angular/core';
import {UrlModel} from "../shared/models/UrlModel";
import {ActivatedRoute, Router} from "@angular/router";
import {UrlsService} from "../shared/services/urls.service";
import {CreateUrlRequest} from "../shared/requests/CreateUrlRequest";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {ApiResponse} from "../shared/responses/ApiResponse";
import {jwtDecode} from "jwt-decode";
import {HttpErrorResponse} from "@angular/common/http";
import {environment} from "../environments/environment";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  protected urls : UrlModel[] = [];
  protected token : string = "";
  protected formGroup: FormGroup = new FormGroup({});
  protected decodedToken: any;
  protected errorMessage: string = "";

  constructor(
    private activatedRoute: ActivatedRoute,
    private urlService: UrlsService,
    private router: Router,
    private fb: FormBuilder)
  {
    this.formGroup = this.fb.group({
      url: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(1000)]],
    });
  }

  ngOnInit(): void {
    this.handleToken()
    const token = localStorage.getItem("token");
    this.loadUrls()
  }

  protected onSubmit(): void {
    if(this.formGroup.valid){
      console.log(this.formGroup.controls['url'].value);
      this.urlService.createUrl(new CreateUrlRequest(this.decodedToken.id, this.formGroup.controls['url'].value)).subscribe(
        (response) => {
          console.log(response);
          this.loadUrls()
        },
        error => {
          console.log(error);
          this.errorMessage = error.error.statusMessage;
        }
      )
    }
    else{
      this.formGroup.markAllAsTouched()
    }
  }

  private handleToken() : void {
    this.activatedRoute.queryParams.subscribe(params => {
      const token = params['token'];
      if (token) {
        this.token = token;
        this.decodedToken = jwtDecode(token);
        console.log(this.decodedToken);
        localStorage.setItem('token', token);
      }
      else{
        localStorage.removeItem('token');
      }
    })
  }

  protected loadUrls(): void {
    this.urlService.getUrls().subscribe(
      (response) => {
        console.log(response);
        this.urls = response.result as UrlModel[];
      },
      error => {
        console.log(error);
      }
    )
  }

  protected onDetailsClick(url: string): string {
    return `${environment.backendUrl}/home/details?url=${url}`;
  }

  protected onDeleteClick(url: string): void {
    this.urlService.deleteUrl(url).subscribe(
      (response) => {
        console.log(response);
        this.loadUrls()
      },
      error => {
        console.log(error);
      }
    );
  }

  protected onDeleteAllClick(): void {
    this.urlService.deleteAllUrls().subscribe(
      (response) => {
        console.log(response);
        this.loadUrls()
      },
      error => {
        console.log(error);
      }
    );
  }

  protected redirectByShortUrl(url: string): string {
    return `${environment.backendUrl}/${url}`;
  }

  protected redirectByLongUrl(url: string): string {
    return `${environment.backendUrl}/Home/ExternalRedirect?externalUrl=${url}`;
  }

  protected redirectToClient(url: string): string {
    return `${environment.backendUrl}/${url}`;
  }

  protected setUrlFormat(url: string): string {
    return `${environment.backendUrl}/${url}`;
  }
}
