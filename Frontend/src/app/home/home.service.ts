import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders  } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { BaseService } from "../shared/base.service";
import { ConfigService } from '../shared/config.service';

@Injectable({
  providedIn: 'root'
})

export class HomeService extends BaseService {

  constructor(private http: HttpClient, private configService: ConfigService) {
    super();
  }


  getLots (page: number, pageSize: number) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json',
      })
    };

    return this.http.get(this.configService.resourceApiURI + '/lots?page=' + page + '&pageSize=' + pageSize, httpOptions).pipe(catchError(this.handleError));
  }


  searchLots (searchQuery: string) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json',
      })
    };

    return this.http.get(this.configService.resourceApiURI + '/lots/search/' + searchQuery, httpOptions).pipe(catchError(this.handleError));
  }


  getCategories () {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json',
      })
    };

    return this.http.get(this.configService.resourceApiURI + '/categories?page=1&pageSize=10', httpOptions).pipe(catchError(this.handleError));
  }


  getLotsByCategory (categoryId: string) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json',
      })
    };

    return this.http.get(this.configService.resourceApiURI + '/categories/' + categoryId + '/lots?page=1&pageSize=10', httpOptions).pipe(catchError(this.handleError));
  }
}
