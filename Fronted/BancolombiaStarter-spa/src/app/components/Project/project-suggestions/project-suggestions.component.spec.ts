import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectSuggestionsComponent } from './project-suggestions.component';

describe('ProjectSuggestionsComponent', () => {
  let component: ProjectSuggestionsComponent;
  let fixture: ComponentFixture<ProjectSuggestionsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectSuggestionsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectSuggestionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
