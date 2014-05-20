LoaderToolkit
=============

Batch loader utility

This toolkit is useful when the following requirements are needed:

Large-sized source files is needed to be loaded to tables of Microsoft Sql Server in an effective manner and it is possibly these files are transformed because of target format (schema of target table).

Fulfilment of this purpose requires the following steps:
1. The bulk load files are created in parallel manner.
2. The contents of bulk load files are inserted to load tables in parallel manner.
3. The load tables are merged to the target table.

The toolkit contains auxiliary projects in addition for facilitation of loading preparation and it contains a Sql Server project of user-defined functions. At present, the toolkit is proposed to load sequences of reference genome, short DNA sequence read alignments and related bioinformatics file contents but possible extension is opened for loading of additional file formats easily. This intention is supported by StructureMap which is a Dependency Injection tool for .NET (see: http://docs.structuremap.net).

This work is supported by the Hungarian Scientific Research Fund: OTKA-103244.