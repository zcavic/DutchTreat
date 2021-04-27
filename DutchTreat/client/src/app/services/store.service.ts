﻿import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { Order, OrderItem } from "../shared/Order";
import { Product } from "../shared/Product"

@Injectable()
export class Store {

	constructor(private http: HttpClient) {

	}

	public products: Product[] = [];
	public order: Order = new Order();
	public token = "";

	loadProducts(): Observable<void> {
		return this.http.get<[]>("/api/products")
			.pipe(map(data => {
				this.products = data;
				return;
			}));
	}

	checkout() {
		const headers = new HttpHeaders().set("Authorization", `Bearer ${this.token}`);

		return this.http.post("/api/orders", this.order, {
			headers: headers
		})
			.pipe(map(() => {
				this.order = new Order();
			}));
	}

	addToOrder(product: Product) {
		let item: OrderItem;
		item = this.order.items.find(o => o.productId === product.id)
		if (item) {
			item.quantity++;
		} else {
			item = new OrderItem();
			item.productId = product.id;
			item.productTitle = product.title;
			item.productArtId = product.artId;
			item.productArtist = product.artist;
			item.productCategory = product.category;
			item.unitPrice = product.price;
			item.productSize = product.size;
			item.quantity = 1;
			this.order.items.push(item)
		}
	}
}