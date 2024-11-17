export class UrlModel {
  constructor(public id : string,
              public shortUrl: string,
              public longUrl: string,
              public createdData: Date,
              public createdBy: string) {
  }
}
