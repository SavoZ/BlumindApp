import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import { CategoryEditModel } from 'src/app/_models/categoryEdit';
import { API_ENDPOINT } from 'src/app/_constants/url.constants';
import { QueryParamsSearch } from 'src/app/_helper.ts/query_params_search';

@Component({
  selector: 'app-edit-category',
  templateUrl: './edit-category.component.html',
  styleUrls: ['./edit-category.component.css']
})
export class EditCategoryComponent implements OnInit {

  constructor(private http: HttpClient, private router: Router, private route: ActivatedRoute) {
  }

  title = 'Category';
  selectedProducts;
  products = [{}];
  model = new CategoryEditModel();

  ngOnInit() {
    this.http.get(API_ENDPOINT + 'Product/GetAllProducts').toPromise().then(result => {
      this.products = result as [{}];
    });
    const id = QueryParamsSearch.findParam(this.route, 'id');
    if (id) {
      this.http.get<CategoryEditModel>(API_ENDPOINT + 'Category/GetProduct', { params: new HttpParams().set('categoryId', id) }).toPromise()
        .then(data => {
          this.model = data;
        });
    }
  }
  addedProduct = (value) => ({ name: value });

  public submit() {
    this.http.post(API_ENDPOINT + 'Category/PostCategory', this.model)
      .toPromise().then(() => {
        // this.router.navigate(['countries']);
      });
  }

}
