using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

using MudBlazor;

using Catalog.Client;

using System.ComponentModel.DataAnnotations;

namespace Catalog.Items
{
    public partial class CommentDialog
    {
        FormModel model = new FormModel();
        bool success;
        
        public class FormModel
        {
            [Required]
            [StringLength(1024, ErrorMessage = "Text length can't be more than 1024.")]
            public string Text { get; set; } = null !;
        }

        [CascadingParameter] MudDialogInstance MudDialog { get; set; }

        [Parameter] public string ItemId { get; set; }

        [Parameter] public string? CommentId { get; set; }

        CommentDto comment;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                if (CommentId is not null)
                {
                    comment = await ItemsClient.GetCommentAsync(ItemId, CommentId);
                    model.Text = comment.Text;
                }
            }
            catch (AccessTokenNotAvailableException exception)
            {
                exception.Redirect();
            }
        }

        async Task OnValidSubmit()
        {
            try
            {
                if (CommentId is null)
                {
                    var item = await ItemsClient.PostCommentAsync(ItemId, new PostCommentDto()
                    {
                        Text = model.Text
                    });
                    
                    MudDialog.Close(DialogResult.Ok(item));
                }
                else
                {
                    await ItemsClient.UpdateCommentAsync(ItemId, CommentId, new UpdateCommentDto()
                    {
                        Text = model.Text
                    });

                    comment.Text = model.Text;

                    MudDialog.Close(DialogResult.Ok(comment));
                }
            }
            catch (AccessTokenNotAvailableException exception)
            {
                exception.Redirect();
            }
            catch (Exception exc)
            {
                Snackbar.Add(exc.Message.ToString(), Severity.Error);
            }
        }

        void Cancel() => MudDialog.Cancel();
    }
}