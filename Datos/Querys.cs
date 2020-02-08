﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class Querys
    {
        private static readonly Querys _instancia = new Querys();
        public static Querys Instancia
        {
            get { return Querys._instancia; }
        }

        public string Query_GuardarVenta()
        {
            string Query = @"
                        begin 
                        declare  @h int, @smsError varchar(500)
                        exec sp_xml_preparedocument @h output , @Cadxml
                        begin try
                        begin transaction
                        IF(SELECT COUNT(*) FROM OpenXML(@h,'root/tbboleta/detalle_tbboleta',1)WITH(
		                           codproducto_detalle int,
		                           cantidad int
		                           )dt INNER JOIN tbstock s on s.CodEstock=dt.codproducto_detalle WHERE s.Stock<dt.cantidad)>0
		                          BEGIN
		                           RAISERROR('Uno ó mas productos no cuentan con el stock suficiente',16,1)
		                          END
		                          INSERT INTO tbboleta(Codboleta,Fechaboleta,Importe,Importe_rg)
                                   SELECT b.codboleta,GETDATE(),b.importe, b.importe_rg
		                           FROM OpenXML(@h,'root/tbboleta',1)WITH(
		                           codboleta nvarchar(20),
		                           importe decimal(5,2),
		                           importe_rg decimal(5,2)
		                           )b
		   
		                           INSERT INTO detalle_tbboleta(Codboleta, Codproducto, CodProducto_detalle, Descripción, Cantidad, Precio_final) 
		                           SELECT dt.codboleta,dt.codproducto,dt.codproducto_detalle,dt.descripcion,dt.cantidad, dt.precio_final
		                           FROM OpenXML(@h,'root/tbboleta/detalle_tbboleta',1)WITH(
		                           codboleta nvarchar(20),
		                           codproducto nvarchar(20),
		                           codproducto_detalle int,
		                           descripcion nvarchar(max),
		                           cantidad int,
		                           precio_final decimal(5,2)
		                           )dt   
		                            
                                    update s
		                               set
		                               s.Stock=s.Stock - st.cantidad
		                               from OpenXML(@h, 'root/tbboleta/tbstock',1)with
		                               (cantidad int,
		                               codestock int)st inner join tbstock s on s.CodEstock=st.codestock

		                           IF(@@TRANCOUNT>0) COMMIT TRANSACTION
		                        END TRY

		                        BEGIN CATCH
		                         IF(@@TRANCOUNT>0)
		                           BEGIN
			                         ROLLBACK TRANSACTION
			                         SELECT @smsError = ERROR_MESSAGE()
			                         RAISERROR(@smsError,16,1)
		                           END
		                        END CATCH
	                         END

                            ";
            return Query;
         }
        public string Query_MostrarInventario()
        {

            string Query = @"if(@tipoBusqueda=1)
                begin
                            select i.Codproducto ,
                            i.Descripción,
                            i.Marca,
                            sum(s.Cantidad) as cantidad_inicial,
                            sum(s.Stock) as Stock, 
                            i.Precio as 'precio/unidad',
                            i.PrecioVenta
                            from tbinventario  i inner join tbstock s 
                            on s.Codproducto=i.Codproducto 
                            where i.Codproducto LIKE '%'+@valorEntrada+'%'
                            group by 
                            i.Codproducto ,i.Descripción, i.Marca, i.Precio, i.PrecioVenta order by Codproducto
                            
                end
                            else if(@tipoBusqueda=2)
                begin
                            select i.Codproducto ,
                            i.Descripción,
                            i.Marca,
                            sum(s.Cantidad) as cantidad_inicial,
                            sum(s.Stock) as Stock, 
                            i.Precio as 'precio/unidad',
                            i.PrecioVenta
                            from tbinventario  i inner join tbstock s 
                            on s.Codproducto=i.Codproducto 
                            where i.Descripción LIKE '%'+@valorEntrada+'%'
                            group by 
                            i.Codproducto ,i.Descripción, i.Marca, i.Precio, i.PrecioVenta order by Codproducto
                            
                end
                            else
                begin
                            select i.Codproducto ,
                            i.Descripción,
                            i.Marca,
                            sum(s.Cantidad) as cantidad_inicial,
                            sum(s.Stock) as Stock, 
                            i.Precio as 'precio/unidad',
                            i.PrecioVenta
                            from tbinventario  i inner join tbstock s 
                            on s.Codproducto=i.Codproducto 
                            where i.Marca LIKE '%'+@valorEntrada+'%'
                            group by 
                            i.Codproducto ,i.Descripción, i.Marca, i.Precio, i.PrecioVenta order by Codproducto
                            
                end

                            ";
            return Query;
        }

        public string Query_ListarInventarioGeneric()
        {
            string Query = @"select i.Codproducto ,
                            i.Descripción,
                            i.Marca,sum(s.Cantidad) as cantidad_inicial,
                            sum(s.Stock) as Stock, 
                            i.Precio as 'precio/unidad',
                            i.PrecioVenta
                            from tbinventario  i inner join tbstock s 
                            on s.Codproducto=i.Codproducto 
                            group by 
                            i.Codproducto ,i.Descripción, i.Marca, i.Precio, i.PrecioVenta order by Codproducto";


            return Query;
        }
        public string Query_TraerDetallePrenda()
        {
            string Query = @"select 
                                  s.CodEstock,
                                  s.Codproducto,
                                i.Descripción,
                                i.Marca,
                                s.Color,
                                s.Talla_alfanum,
                                s.Talla_num,
                                s.Cantidad,
                                i.PrecioVenta,
                                s.Stock from tbstock s inner join tbinventario i on s.Codproducto= i.Codproducto where s.Codproducto=@Codproducto";

            return Query;
        }
        public string Query_AgregarProductoBoleta()
        {
            string Query = @"select 
                                  s.CodEstock,
                                  s.Codproducto,
                                i.Descripción,
                                i.Marca,
                                s.Color,
                                s.Talla_alfanum,
                                s.Talla_num,
                                s.Cantidad,
                                s.Stock,
                                i.PrecioVenta,
                                s.Stock from tbstock s inner join tbinventario i on s.Codproducto= i.Codproducto where s.CodEstock=@Codproducto";

            return Query;
        }

        public string Query_GuardarInventario()
        {
            string Query = @"
                    begin
                        declare @h int , @msmError varchar(500)
                        exec sp_xml_preparedocument @h output, @Cadxml
                        begin try
                        begin transaction
                        insert into  tbinventario(Codproducto, Descripción, Marca, Precio, PrecioVenta)
                        select i.codproducto, i.descripcion, i.marca, i.precio, i.precioventa
                        from openxml(@h, 'root/tbinventario', 1)with
                        (
                        codproducto nvarchar(20),
                        descripcion nvarchar(max),
                        marca nvarchar(max),
                        precio decimal(5,2),
                        precioventa decimal(5,2)
                        )i
                        insert into  tbstock(Codproducto, Color, Talla_alfanum, Talla_num, Cantidad, Stock )
                        select s.codproducto, s.color, s.talla_alfanum, s.talla_num, s.cantidad, s.stock
                        from openxml(@h, 'root/tbinventario/tbstock',1)with
                        (
                        codproducto nvarchar(20),
                        color nvarchar(max),
                        talla_alfanum nvarchar(max),
                        talla_num int ,
                        cantidad int,
                        stock int
                        )s
                        if (@@trancount>0)commit transaction
                        end try
                        begin catch
                        if(@@trancount>0)
                        begin
                        rollback transaction
                        select @msmError=ERROR_MESSAGE()
                        raiserror(@msmError,16,1)
                        end 
                        end catch
                        end
    
                    ";
            return Query;
        }
        public string Query_MostrarVentasFecha()
        {
            string Query = @"
                            select 
                            dt.Codboleta,
                            sum(dt.Cantidad)as Prendas,
                            sum(dt.Precio_final)as Total,
                            b.Fechaboleta
                            from detalle_tbboleta dt inner join tbboleta b
                            
                            on b.Codboleta=dt.Codboleta
                            where b.Fechaboleta=@Fechaboleta
                            group by 
                            dt.Codboleta,
                            dt.Cantidad,
                            b.Fechaboleta
                            order by Fechaboleta
                            ";

            return Query;
        }
        public string Query_MostrarVentasFechaDoble()
        {
            string Query = @"
                            select 
                            dt.Codboleta,
                            sum(dt.Cantidad)as Prendas,
                            sum(dt.Precio_final)as Total,
                            b.Fechaboleta
                            from detalle_tbboleta dt inner join tbboleta b
                            
                            on b.Codboleta=dt.Codboleta
                            where b.Fechaboleta between @FechaBoletaIni and @FechaBoletaFin
                            group by 
                            dt.Codboleta,
                            dt.Cantidad,
                            b.Fechaboleta
                            order by Fechaboleta
                            ";
            return Query;
        }
        public string Query_BuscarBoletaVenta()
        {
            string Query = @"
                            select 
                            b.Codboleta,
                            sum(dt.Cantidad)as Prendas,
                            --sum(dt.Precio_final)as Total,
                            b.Importe_rg as Total, 
                            b.Fechaboleta
                            from tbboleta b inner join detalle_tbboleta dt
                            
                            on b.Codboleta=dt.Codboleta
                            where b.Codboleta like '%'+ @Codboleta +'%'
                            group by 
                            b.Codboleta,
                            dt.Cantidad,
                            --dt.Precio_final,
                            b.Importe_rg,
                            b.Fechaboleta
                            order by Fechaboleta
                            ";
            return Query;
        }
        public string Query_ListarDetalleVenta()
        {
            string Query = @"
                                select
                                detalle_tbboleta.Codproducto, 
                                tbinventario.Descripción, 
                                tbinventario.Marca, 
                                tbstock.Color, 
                                tbstock.Talla_alfanum, 
                                tbstock.Talla_num,
                                detalle_tbboleta.Cantidad,  
                                detalle_tbboleta.Precio_final

                                from tbinventario inner join detalle_tbboleta   

                                on  tbinventario.Codproducto=detalle_tbboleta.Codproducto 
                                inner join tbstock on tbstock.Codproducto =tbinventario.Codproducto  
                                inner join tbboleta on tbboleta.Codboleta=detalle_tbboleta.Codboleta   
                                
                                where 
                                detalle_tbboleta.Codboleta=@Codboleta and 
                                tbstock.CodEstock =detalle_tbboleta.Codproducto_detalle and 
                                detalle_tbboleta.Coddetalle =detalle_tbboleta.Coddetalle  
                            ";

            return Query;
        }
        public string Query_MantenimientoDetalleInventario()
        {
            string Query = @"
                                begin
                                declare @h int, @error nvarchar(500)
                                exec sp_xml_preparedocument @h output, @Cadxml 
                                begin try
                                begin transaction

                                insert into tbstock(Codproducto, Color, Talla_alfanum, Talla_num, Cantidad, Stock)
                                select s.codproducto, s.color, s.talla_alfanum, s.talla_num, s.cantidad, s.stock
                                from openxml(@h,'root/tbstock', 1)with
                                (
                                codproducto nvarchar(20),
                                color nvarchar(max),
                                talla_alfanum nvarchar(max),
                                talla_num int,
                                cantidad int,
                                stock int,
                                tipoaccion int
                                )s where tipoaccion=1

                                update st 
                                set
                                st.Color=s.color,
                                st.Talla_alfanum=s.talla_alfanum,
                                st.Talla_num=s.talla_num,
                                st.Cantidad=s.cantidad,
                                st.Stock=s.stock

                                from openxml (@h,'root/tbstock',1)with
                                (
                                codestock int,
                                color nvarchar(max),
                                talla_alfanum nvarchar(max),
                                talla_num int, 
                                cantidad int ,
                                stock int ,
                                tipoaccion int

                                )s inner join tbstock st on s.codestock=st.CodEstock where tipoaccion=2

                                delete from tbstock
                                from openxml(@h, 'root/tbstock', 1)with
                                (
                                codestock int,
                                tipoaccion int
                                ) s inner join tbstock st on s.codestock=st.CodEstock where tipoaccion=3
                                if(@@trancount>0) commit transaction
                                end try
                                begin catch
                                if(@@trancount>0) rollback transaction
                                select @error=error_message()
                                raiserror(@error,16,1)
                                end catch
                                end

                            ";


            return Query;
        }
    }
}
