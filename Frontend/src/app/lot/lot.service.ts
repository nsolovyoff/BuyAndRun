import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders  } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { BaseService } from "../shared/base.service";
import { ConfigService } from '../shared/config.service';

@Injectable({
  providedIn: 'root'
})

export class LotService extends BaseService {

  constructor(private http: HttpClient, private configService: ConfigService) {
    super();
  }

  getLot(token: string, lotId: string) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json',
        'Authorization': token
      })
    };

    return this.http.get(this.configService.resourceApiURI + '/lots/' + lotId, httpOptions).pipe(catchError(this.handleError));
  }


  getCategories () {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json',
      })
    };

    return this.http.get(this.configService.resourceApiURI + '/categories?page=1&pageSize=10', httpOptions).pipe(catchError(this.handleError));
  }

  buyNow(token: string, lotId: string) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json',
        'Authorization': token
      })
    };

    return this.http.put(this.configService.resourceApiURI + '/lots/' + lotId + '/buy-now',{}, httpOptions).pipe(catchError(this.handleError));
  }

  makeBid(token: string, userId : string, bid : string, lotId: string) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json',
        'Authorization': token
      })
    };

    const body = {
      "bid": Number(bid),
      "userId": userId
    };

    return this.http.put(this.configService.resourceApiURI + '/lots/' + lotId + '/make-bid', body, httpOptions).pipe(catchError(this.handleError));
  }

  getOwner(token: string, lotId: string) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json',
        'Authorization': token
      })
    };

    return this.http.get(this.configService.resourceApiURI + '/lots/' + lotId + '/owner', httpOptions).pipe(catchError(this.handleError));
  }

  createLot(token: string, title: string, description: string, buyNowPrice: string, bid: string, expiring: string, categoryId: string,
            user: string, imageBase64: string) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json',
        'Authorization': token
      })
    };


    const lotBody = {
      "title": title,
      "description": description,
      "buyNowPrice": Number(buyNowPrice),
      "bid": Number(bid),
      "bidUser": user,
      "expiring": new Date(expiring).toISOString(),
      "categoryId": Number(categoryId),
      "imageBase64": imageBase64,
      "user": user
    };

    return this.http.post(this.configService.resourceApiURI + '/lots', lotBody, httpOptions).pipe(catchError(this.handleError));
  }

  deleteLot(token: string, lotId: string) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json',
        'Authorization': token
      })
    };

    return this.http.delete(this.configService.resourceApiURI + '/lots/' + lotId, httpOptions).pipe(catchError(this.handleError));
  }
}
