# Onboarding System

**Developed by [Batuhan Uysal](https://github.com/ncrx7), `Onboarding System Package` is Reusable feature-onboarding (FTUE) system: queued "new feature" popups, unmask-overlay
spotlight tutorials, signal/timer based dismiss conditions and a Zenject-driven action pipeline.**

You can set onboarding your new features to your game easily by using this package. 

> — Batuhan Uysal, Senior Game Developer

## External dependencies

Resolved automatically by UPM (declared in `package.json`):

- `com.cysharp.unitask`
- `com.coffee.unmask` (UnmaskForUGUI)
- `com.unity.addressables`

Must already be present in the consuming project (Asset Store / manual import, no asmdef
required, must compile alongside `Assembly-CSharp`):

- **Zenject** (DI container)
- **Odin Inspector (Sirenix)** — required for `OnboardingFeatureData : SerializedScriptableObject`,
  which serializes the polymorphic `List<IOnboardingAction>` / dismiss condition fields.

This package intentionally has **no Assembly Definition**. Zenject and Odin ship without
asmdefs in most projects (they live in `Assembly-CSharp`), so an asmdef'd package could not
reference them. Scripts here compile together with the rest of the project, same as today.

## Project integration

Implement two adapters in your project and bind them in your Zenject installer:

- `IOnboardingLevelProvider` — exposes `CurrentLevelIndex` and an `OnLevelReady` event from
  your level/game-flow system.
- `IOnboardingSaveService` — loads/saves `OnboardingData` (claimed feature ids) through your
  save system.

Then bind:

```csharp
Container.BindInterfacesAndSelfTo<YourLevelProviderAdapter>().AsSingle();
Container.BindInterfacesAndSelfTo<YourSaveServiceAdapter>().AsSingle();
Container.BindInterfacesAndSelfTo<OnboardingHighlightRegistry>().AsSingle();
Container.BindInterfacesAndSelfTo<NewFeaturePopUp>().FromComponentInHierarchy().AsSingle();
Container.BindInterfacesAndSelfTo<UnmaskOverlayPresenter>().FromComponentInHierarchy().AsSingle();
Container.BindInterfacesAndSelfTo<OnboardingManager>().AsSingle();
```

Project-specific signal id constants (e.g. "booster cast" signals) are NOT part of this
package — define them in your project alongside `OnboardingSignals`.

## Content

- `Interfaces/` — `IOnboardingManager`, `IOnboardingAction`, `IOnboardingDismissCondition`,
  `IOnboardingHighlightRegistry`, `IUnmaskOverlayPresenter`, `IOnboardingLevelProvider`,
  `IOnboardingSaveService`
- `Core/` — `OnboardingManager`, `OnboardingFeatureData`, `OnboardingData`,
  `OnboardingHighlightRegistry`, `OnboardingHighlightAnchor`, `OnboardingSignals`
- `Presentation/` — `UnmaskOverlayPresenter`, `NewFeaturePopUp`
- `Actions/` — `UnmaskedPresentAction`, `UnmaskedWorldPresentAction`
- `Conditions/` — `TimerDismissCondition`, `SignalDismissCondition`

---

## 👨‍💻 Author

**Batuhan Uysal**  
Senior Game Developer — [batuhanuysal.com](https://batuhanuysal.com)  
[GitHub](https://github.com/ncrx7) | [LinkedIn](https://www.linkedin.com/in/batuhan-uysal-39596021a/) 
| [YouTube](https://www.youtube.com/@ncrx7staticriver)

---

## 📄 License

MIT © Batuhan Uysal 
Use freely in personal and commercial projects. Attribution appreciated but not required.
