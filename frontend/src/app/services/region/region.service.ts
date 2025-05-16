import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {SubcategoryResponse} from '../../models/subcategory/subcategory-response';
import {Observable, Subscriber} from 'rxjs';
import {CreateSubcategoryRequest} from '../../models/subcategory/create-subcategory-request';
import {UpdateSubcategoryRequest} from '../../models/subcategory/update-subcategory-request';
import {RegionResponse} from '../../models/region/region-response';

@Injectable({
  providedIn: 'root'
})
export class RegionService {
  http = inject(HttpClient);

  baseApiUrl = "https://darom.md/"
  testApiUrl = "http://localhost:5079/api/v1"


  private regions: Array<RegionResponse> = [
    {id: 1, name: "Chișinău"},
    {id: 2, name: "Bălți"},
    {id: 3, name: "Orhei"},
    {id: 4, name: "Cahul"},
    {id: 5, name: "Soroca"},
    {id: 6, name: "Strășeni"},
    {id: 7, name: "Drochia"},
    {id: 8, name: "Florești"},
  ]

  get(): Observable<Array<RegionResponse>> {
    return Observable.create((observer: Subscriber<any>) => {
      observer.next(this.regions);
      observer.complete();
    });
    // return Observable<this.categories>;
    // return this.http.get<Doctor[]>(`${this.testApiUrl}/doctors`)
  }

  getById(id: number) {
    // return this.http.get<Doctor>(`${this.testApiUrl}/doctors/${id}`)
  }

  getByCategoryId(id: number) {
    // return this.http.get<Doctor>(`${this.testApiUrl}/doctors/${id}`)
  }

  create(request: CreateSubcategoryRequest) {
    // return this.http.post(`${this.testApiUrl}/doctor`, {
    //   "Name": request.Name,
    //   "Surname": request.Surname,
    //   "PhotoPath": request.PhotoPath,
    //   "SpecialityId": request.SpecialityId
    // });
  }

  update(request: UpdateSubcategoryRequest) {
    // return this.http.put(`${this.testApiUrl}/doctors`, {
    //   "Id": request.Id,
    //   "Name": request.Name,
    //   "Surname": request.Surname,
    //   "PhotoPath": request.PhotoPath,
    //   "SpecialityId": request.SpecialityId,
    // });
  }

  delete(id: number) {
    // return this.http.delete(`${this.testApiUrl}/doctors?id=${id}`)
  }
}
