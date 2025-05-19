namespace BaCS.Presentation.MAUI.ViewModels.States;

using Services;

public class ReservationResourceState
{
    private Client client;

    public ReservationResourceState(Client client)
    {
        this.client = client;
    }

    public ResourceDto? SelectedResource { get; set; }

    public UpdateReservationRequest? Request { get; set; }

    public ReservationDto? ReservationResult { get; set; }

    public async Task SendRequestAsync()
    {
        try
        {
            if (SelectedResource == null || Request == null)
            {
                ReservationResult = null;

                return;
            }

            ReservationResult = await client.ReservationsPUTAsync(SelectedResource.Id, Request);
        }
        catch (Exception e)
        {
            ReservationResult = null;
        }
    }
}
