export class ApiResponse {
  constructor(public isSucceeded: boolean,
              public statusCode: string,
              public statusMessage: string,
              public result: Object) {
  }
}
