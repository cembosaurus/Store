import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ItemsAlbumComponent } from './items-album.component';

describe('ItemsAlbumComponent', () => {
  let component: ItemsAlbumComponent;
  let fixture: ComponentFixture<ItemsAlbumComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ItemsAlbumComponent]
    });
    fixture = TestBed.createComponent(ItemsAlbumComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
