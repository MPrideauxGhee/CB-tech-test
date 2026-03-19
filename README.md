# Payment Service Refactor

## What Changed and Why
### `IAccountDataStore`
Added to the `Data/` folder alongside the existing implementations. `AccountDataStore` and `BackupAccountDataStore` both now implement this without any changes to their internal logic. This allows the datastores to be mocked in tests and satisfies the **Dependency Inversion Principle**. `PaymentService` now depends on an abstraction rather than a concretion.

### `IAccountDataStoreFactory` / `AccountDataStoreFactory`
The original code duplicated the datastore selection logic twice in the same method. This logic has been extracted into a factory, centralising the decision in one place. Configuration accessed inline via `ConfigurationManager` is now provided via `IOptions<DataStoreOptions>`, removing the dependency on static configuration and making the behaviour easier to test.

### `IPaymentValidator` / `<AllowedPaymentSchemes>PaymentValidator`
The original switch statement mixed null, flag, balance, and status checks all in one block. Each validator now owns the logic for exactly one payment scheme. `PaymentService` holds a dictionary of validators keyed by scheme and delegates to the correct one.

This satisfies:
- **Single Responsibility** - each validator has one job
- **Open/Closed** - adding a new payment scheme means adding a new validator class. `PaymentService` doesn't change
- **Readability** - the rules for each scheme are visible and self-contained

### `PaymentService`
Reduced to its core responsibility. It retrieves the account, guards against a null account, delegates validation to the appropriate validator, and if successful delegates the balance update and persistence to `ITransactionService`. All dependencies are injected via the constructor, making the class fully testable with no external dependencies or configuration.

This now follows a fail‑fast approach. Instead of defaulting Success = true and mutating it through multiple branches, the method returns immediately when a validation step fails. This makes the control flow explicit, reduces branching, and keeps the happy path clear and readable.

### `MakePaymentResult`
The Success property now defaults to false, which reflects the natural assumption that a payment is unsuccessful until all validation and processing steps complete successfully.

### `ITransactionService` / `TransactionService`
Balance mutation and persistence logic have been extracted into a dedicated `TransactionService`, centralising all account state changes.

`TransactionService` now owns:
- Deducting the payment amount from the account
- Persisting the updated account via `IAccountDataStore`
- Wrapping persistence failures in a domain-specific `TransactionException`

This further enforces **Single Responsibility** - `PaymentService` coordinates the flow, while `TransactionService` performs the state change. It also improves testability by isolating side effects behind a single abstraction.

The service is open for extension. Additional controls/fucntionalities can be added without modifying `PaymentService`.

### `Dependencies.cs`
All dependencies are registered via a central `Dependencies` class, split into focused private methods - `RegisterDataStores`, `RegisterValidators`, and `RegisterServices`.

Validators are composed into a singleton dictionary keyed by `PaymentScheme`. A startup validation step asserts that every `PaymentScheme` enum value has a corresponding registered validator. If any are missing the application throws `InvalidOperationException` at launch rather than failing silently at runtime.

## Testing Approach
The refactored design enables unit testing by:
- removing static and hard-coded dependencies
- introducing interfaces for all external interactions
- allowing mocking of data store access, transaction execution, and validation logic

Tests are written with **xUnit**, **FluentAssertions** and **Moq**.

| Test Class | What it covers |
|---|---|
| `<AllowedPaymentSchemes>PaymentValidatorTests` | AllowedPaymentScheme business rules in isolation |
| `AccountDataStoreFactoryTests` | Correct data store returned for each config value |
| `PaymentServiceTests` | Correct validator called, `Execute` called only on success, null account handled |
| `TransactionServiceTests` | Balance deduction, `UpdateAccount` called, `TransactionException` wrapping |

Validator tests require no mocking - they are pure input/output assertions against business rules. Service tests use mocks to verify behaviour rather than state, which is appropriate since `PaymentService` is an orchestrator.

Tests are written to express intent rather than implementation details. For example, `PaymentServiceTests` assert that the correct validator is invoked, not how validation is implemented. This ensures tests remain stable even as internal logic evolves.

## What I Would've Done With More Time
- **ValidatorDictionaryBuilder** - extracting validator dictionary construction into its own class would make it independently testable.
- **Input validation on `MakePaymentRequest`** - the current code assumes a well-formed request. A null request or missing account number would produce an unhelpful exception
- **Doc everything** - initial doc examples are in place, but would extend such that the code is better documented.
- **Logging** - the service currently gives no visibility into why a payment failed. Structured logging at key decision points would be valuable, including validator failures, or per-payment scheme metrics.
- **Guard against invalid enum values** - while `PaymentScheme` is an enum and in normal usage only valid values will arrive, a cast from an arbitrary integer is possible; a boundary-level guard on deserialisation would close that gap.
- **BDD-style tests** - to ensure that the application behaves as expected from a user perspective e.g. specflow