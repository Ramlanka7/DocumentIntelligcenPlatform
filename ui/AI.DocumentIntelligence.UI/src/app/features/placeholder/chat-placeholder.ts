import { ChangeDetectionStrategy, Component } from '@angular/core';

import { FeaturePlaceholder } from './feature-placeholder';

/** Route-level placeholder for `/chat` — feature internals land in T12. */
@Component({
  selector: 'app-chat-placeholder',
  standalone: true,
  imports: [FeaturePlaceholder],
  template: `<app-feature-placeholder
    icon="forum"
    title="AI-Powered Document Chat"
    description="Document chat is coming soon. Ask questions and get cited answers grounded in your documents."
  />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ChatPlaceholder {}
