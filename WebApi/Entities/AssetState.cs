namespace WebApi.Entities;

public enum AssetState {
    Assigned,
    Available,
    NotAvailable,
    WaitingForRecycling,
    Recycled,
}