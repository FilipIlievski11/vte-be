using Microsoft.EntityFrameworkCore;
using VTE.Infrastructure.Persistence;

namespace VTE.Api.Services;

/// <summary>
/// Живо читање на легаси-максимумот на една нумерациска серија, преку linked
/// server-от <c>VTEZVV_LIVE</c> на прод SQL-от (истиот што го користи синк-от).
///
/// Зошто: легаси и v2 работат ПАРАЛЕЛНО и ја делат истата серија на броеви, а
/// огледалото во v2 се освежува само при синхронизација (2× дневно). MAX-от од
/// v2 табелата затоа доцни со часови зад легаси — на 05.08.2026 тоа произведе
/// дупликат број (01-37-48125/2026 во двата система). Пред доделување број,
/// серијата се проверува и во легаси; конечниот број е поголемиот од двата
/// максимума + 1.
///
/// Отпорност: ако linked server-от го нема (околина без него / легаси
/// недостапно), падот се голта со warning и нумерацијата продолжува само од
/// v2 серијата — однесувањето од пред фиксот.
/// </summary>
public static class LegacyNumbering
{
    /// <summary>MAX секвенца во легаси PaymentDocuments за серија со дадена
    /// LIKE-шема (пр. „01-37-%/2026"). Форматот е [prefix-]org-seq/year;
    /// секвенцата е сегментот меѓу последната цртичка и косата црта.</summary>
    public static async Task<long> MaxBillSeqAsync(VteDbContext db, string seriesLike, ILogger? log, CancellationToken ct = default)
    {
        try
        {
            var row = await db.Database.SqlQuery<long?>($@"
                SELECT MAX(TRY_CAST(LEFT(tail, CHARINDEX('/', tail) - 1) AS bigint)) AS [Value]
                FROM (
                    SELECT RIGHT(DocumentNumber, CHARINDEX('-', REVERSE(DocumentNumber)) - 1) AS tail
                    FROM VTEZVV_LIVE.VTEZVV.dbo.PaymentDocuments
                    WHERE DocumentNumber LIKE {seriesLike}
                      AND CHARINDEX('-', DocumentNumber) > 0
                      AND CHARINDEX('/', DocumentNumber) > 0
                ) t
                WHERE CHARINDEX('/', tail) > 1").FirstOrDefaultAsync(ct);
            return row ?? 0;
        }
        catch (Exception ex)
        {
            log?.LogWarning(ex, "Легаси нумерација (сметки, серија {Series}) недостапна — продолжувам само со v2 серијата.", seriesLike);
            return 0;
        }
    }

    /// <summary>MAX секвенца во легаси DocumentsTehnicalExamsReports за рег.
    /// броеви со шема „{org}-%/{year}" (формат org-seq/year).</summary>
    public static async Task<long> MaxExamSeqAsync(VteDbContext db, string seriesLike, ILogger? log, CancellationToken ct = default)
    {
        try
        {
            var row = await db.Database.SqlQuery<long?>($@"
                SELECT MAX(TRY_CAST(LEFT(tail, CHARINDEX('/', tail) - 1) AS bigint)) AS [Value]
                FROM (
                    SELECT RIGHT(RegNumber, CHARINDEX('-', REVERSE(RegNumber)) - 1) AS tail
                    FROM VTEZVV_LIVE.VTEZVV.dbo.DocumentsTehnicalExamsReports
                    WHERE RegNumber LIKE {seriesLike}
                      AND CHARINDEX('-', RegNumber) > 0
                      AND CHARINDEX('/', RegNumber) > 0
                ) t
                WHERE CHARINDEX('/', tail) > 1").FirstOrDefaultAsync(ct);
            return row ?? 0;
        }
        catch (Exception ex)
        {
            log?.LogWarning(ex, "Легаси нумерација (прегледи, серија {Series}) недостапна — продолжувам само со v2 серијата.", seriesLike);
            return 0;
        }
    }
}
